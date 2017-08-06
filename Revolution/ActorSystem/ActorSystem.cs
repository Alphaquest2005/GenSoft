using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using BuildingSpecifications;
using Common.Dynamic;
using DataServices.Actors;
using DynamicExpresso;
using GenSoft.DBContexts;
using GenSoft.Expressions;
using Microsoft.EntityFrameworkCore;
using Process.WorkFlow;
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;

namespace ActorBackBone
{
    [Export(typeof(IActorBackBone))]
    public class ActorBackBone: IActorBackBone
    {
        //ToDo:Get rid of private setter
        public static ActorBackBone Instance { get; private set; }
       
        public static ActorSystem System { get; private set; }


        public void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<IProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos, Assembly interfaces)
        {
             try
            {
                System = ActorSystem.Create("System");
                var res = GetDBComplexActions(interfaces);
                res.AddRange(complexEventActions);
                System.ActorOf(Props.Create<ServiceManager>(autoRun,machineInfo, processInfos, res,viewInfos),"ServiceManager");
                Instance = this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IComplexEventAction> GetDBComplexActions(Assembly interfaces)
        {
            try
            {





                var res = new List<IComplexEventAction>();
                using (var ctx = new GenSoftDBContext())
                {
                    foreach (var r in ctx.DomainEntityType
                        .Include(x => x.DomainEntityTypeSourceEntity)
                        .Include(x => x.EntityType.Type)
                        .Include(x => x.EntityType.EntityView)
                        .Include(x => x.EntityType.DomainEntityType.DomainEntityCache)
                        .Include(x => x.EntityType.EntityList)
                        .Include("ProcessStateDomainEntityTypes.ProcessState.Process")
                        .OrderBy(x => x.Id)
                        )
                    {
                        var entityType = interfaces.GetTypes().FirstOrDefault(x => x.Name == r.EntityType.Type.Name);
                        //var childExpression = typeof(ExpressionsExtensions).GetMethod("BuildForType").MakeGenericMethod(entityType)
                        //    .Invoke(null, new object[] { entityType, r.EntityType.EntityTypeAttributes.FirstOrDefault(x => x.EntityId != null).Name });
                        foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            if (r.EntityType.EntityView != null)
                            {
                                if (r.EntityType.EntityList == null)
                                {

                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateState", new[] {entityType},
                                        new object[] {processId}));
                                }
                                else
                                {
                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList",
                                        new[] {entityType}, new object[] {processId}));
                                }
                            }

                            if(r.EntityType.DomainEntityType.DomainEntityCache != null && r.EntityType.EntityView == null)
                                res.Add(EntityComplexActions.GetComplexAction("IntializeCache", new[] { entityType }, new object[] { processId }));

                            if (r.EntityType.DomainEntityType.DomainEntityCache != null && r.EntityType.EntityView != null)
                                res.Add(EntityViewComplexActions.GetComplexAction("IntializeCache", new[] { entityType }, new object[] { processId }));

                        }
                    }

                    foreach (var r in ctx.EntityRelationships
                        .Include(x => x.ChildEntity.EntityType.Type)
                        .Include(x => x.ChildEntity.EntityType.EntityList)
                        .Include(x => x.ParentEntity.EntityType.Type)
                        .Include(x => x.ParentEntity.EntityType.EntityList)
                        .Include(x => x.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity)
                        .Include(
                            "ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                        .OrderBy(x => x.ParentEntity.Id)
                        .Where(x => x.ChildEntity.EntityType.CompositeRequest == null))
                    {
                        var parentType = interfaces.GetTypes()
                            .FirstOrDefault(x => x.Name == r.ParentEntity.EntityType.Type.Name);
                        var childType = interfaces.GetTypes()
                            .FirstOrDefault(x => x.Name == r.ChildEntity.EntityType.Type.Name);
                        var sourceEntityName = r.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity
                            ?.SourceEntity;

                        var parentExpression = typeof(ExpressionsExtensions).GetMethod("BuildForType")
                            .MakeGenericMethod(parentType)
                            .Invoke(null, new object[] {parentType, r.ParentEntity.Name});

                        var childExpression = typeof(ExpressionsExtensions).GetMethod("BuildForType")
                            .MakeGenericMethod(childType)
                            .Invoke(null, new object[] {childType, r.ChildEntity.Name});



                        //var childPropertyExpression = ExpressionsExtensions.Build(childType, r.ChildEntity.EntityTypeAttributes.Name);




                        foreach (var processId in r.ChildEntity.EntityType.DomainEntityType
                            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            if (sourceEntityName != null)
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestPulledState",
                                    new[] {parentType, childType}, new object[] {processId, sourceEntityName}));
                            }
                            else
                            {
                                if (r.ChildEntity.EntityType.EntityList == null)
                                    res.Add(Processes.ComplexActions.GetComplexAction("RequestState",
                                    new[] {parentType, childType}, new object[] {processId, childExpression}));
                            }

                            if (r.ChildEntity.EntityType.EntityList != null)
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestStateList",
                                    new[] {parentType, childType},
                                    new object[] {processId, parentExpression, childExpression}));
                            }

                            res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",
                                new[] {parentType, childType},
                                new object[] {processId, parentExpression, childExpression}));
                        }

                    }


                    foreach (var g in ctx.EntityRelationships
                        .Include("ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                        .Where(x => x.ChildEntity.EntityType.CompositeRequest != null)
                        .GroupBy(x => x.ChildEntity.EntityType).Where(g => g.Count() > 1))                        
                    {

                        foreach (var processId in g.Key.DomainEntityType
                            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            var childType = interfaces.GetTypes().FirstOrDefault(x => x.Name == g.Key.Type.Name);
                            var parentEntities = g.Select(p =>
                                                new ViewModelEntity() {
                                                    EntityType = interfaces.GetTypes().FirstOrDefault(x => x.Name == p.ParentEntity.EntityType.Type.Name),
                                                    Property = p.ChildEntity.Name}
                                                    ).ToList();

                            res.Add(Processes.ComplexActions.GetComplexAction("RequestCompositeStateList",
                                new[] { childType },
                                new object[] { processId, new Dictionary<string, dynamic>(), parentEntities }));
                        }
                    }


                    }
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        public void Intialize(bool autoContinue, List<IViewModelInfo> viewInfos)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var machineInfo = ctx.Machine.Select(x => ProcessExpressions.CreateMachineInfo(x)).ToList();

                var processInfos = ctx.Process.Select(x => ProcessExpressions.CreateProcessInfo(x)).ToList();
                List<IComplexEventAction> dbComplexAction = new List<IComplexEventAction>(); 
                //var dbComplexAction = ctx.ProcessComplexState
                //    .Select(x => new BuildingSpecification<ComplexEventAction>()
                //    {
                //        Properties = new PropertyBag()
                //        {
                //            {nameof(IComplexEventAction.Key), x.ProcessState.Name },
                //            {nameof(IComplexEventAction.ProcessId), x.ProcessState.ProcessId },
                //            {nameof(IComplexEventAction.ExpectedMessageType), null },//ToDo: define MessageType
                //            {nameof(IComplexEventAction.ActionTrigger), Enum.Parse(typeof(ActionTrigger), x.ProcessStateTrigger.Name) },
                //            {nameof(IComplexEventAction.ProcessInfo),
                //                new BuildingSpecification<ProcessInfo>(){
                //                    Properties = new PropertyBag()
                //                    {
                //                        {nameof(IProcessInfo.Name), x.ProcessState.Process.Name },
                //                        {nameof(IProcessInfo.Description), x.ProcessState.Process.Description },
                //                        {nameof(IProcessInfo.ParentProcessId), x.ProcessState.Process.ParentProcessId },
                //                        {nameof(IProcessInfo.Symbol), x.ProcessState.Process.Symbol },
                //                        {nameof(IProcessInfo.Id), x.ProcessState.Process.Id },
                //                        {nameof(IProcessInfo.UserId), x.ProcessState.Process.UserId },
                //                    }}.Build() },
                //            {nameof(IComplexEventAction.Action),
                //                new BuildingSpecification<ProcessAction>()
                //                {
                //                    Properties = new PropertyBag()
                //                    {
                //                        {nameof(IProcessAction.ExpectedSourceType), new SourceType(typeof(IComplexEventService)) },
                //                        {nameof(IProcessAction.Action), ProcessActions
                //                         },
                //                    }
                //                } },

                //        }
                //    }

                //                    .Build() as IComplexEventAction).ToList();


                Intialize(autoContinue, machineInfo, processInfos, dbComplexAction, viewInfos, null);
            }

            
        }
    }
}
