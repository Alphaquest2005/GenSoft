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


        public void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<IProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos)
        {
             try
            {
                System = ActorSystem.Create("System");
                var res = GetDBComplexActions();
                res.AddRange(complexEventActions);
                System.ActorOf(Props.Create<ServiceManager>(autoRun,machineInfo, processInfos, res,viewInfos),"ServiceManager");
                Instance = this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IComplexEventAction> GetDBComplexActions()
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
                        var entityType = r.EntityType.Type.Name;
                        //var childExpression = typeof(ExpressionsExtensions).GetMethod("BuildForType").MakeGenericMethod(entityType)
                        //    .Invoke(null, new object[] { entityType, r.EntityType.EntityTypeAttributes.FirstOrDefault(x => x.EntityId != null).Name });
                        foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            if (r.EntityType.EntityView != null)
                            {
                                if (r.EntityType.EntityList == null)
                                {

                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateState", new object[] {processId,entityType}));
                                }
                                else
                                {
                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList", new object[] {processId,entityType}));
                                }
                            }

                            if(r.EntityType.DomainEntityType.DomainEntityCache != null )
                                res.Add(EntityComplexActions.GetComplexAction("IntializeCache",  new object[] { processId, entityType}));

                            

                        }
                    }

                    foreach (var r in ctx.EntityRelationships
                        .Include(x => x.ChildEntity.EntityType.Type)
                        .Include(x => x.ChildEntity.Attributes)
                        .Include(x => x.ChildEntity.EntityType.EntityList)
                        .Include(x => x.ParentEntity.EntityType.Type)
                        .Include(x => x.ParentEntity.Attributes)
                        .Include(x => x.ParentEntity.EntityType.EntityList)
                        .Include(x => x.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity)
                        .Include(
                            "ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                        .OrderBy(x => x.ParentEntity.Id)
                        .Where(x => x.ChildEntity.EntityType.CompositeRequest == null))
                    {
                        var parentType = r.ParentEntity.EntityType.Type.Name;
                        var childType = r.ChildEntity.EntityType.Type.Name;
                        var sourceEntityName = r.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity
                            ?.SourceEntity;

                        var parentExpression = r.ParentEntity.Attributes.Name;

                        var childExpression = r.ChildEntity.Attributes.Name;



                        //var childPropertyExpression = ExpressionsExtensions.Build(childType, r.ChildEntity.EntityTypeAttributes.Name);




                        foreach (var processId in r.ChildEntity.EntityType.DomainEntityType
                            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {

                            if (r.ChildEntity.EntityType.EntityList == null)
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestState",
                                    new object[] {processId, parentType, childType, childExpression}));
                            }
                            else
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestStateList",new object[] {processId,parentType, childType, parentExpression, childExpression}));
                            }
                            res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new object[] {processId,parentType, childType, parentExpression, childExpression}));
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
                            var childType = g.Key.Type.Name;
                            var parentEntities = g.Select(p =>
                                                new ViewModelEntity() {
                                                    EntityType = p.ParentEntity.EntityType.Type.Name,
                                                    Property = p.ChildEntity.Attributes.Name}
                                                    ).ToList();

                            res.Add(Processes.ComplexActions.GetComplexAction("RequestCompositeStateList",
                                new object[] { processId, childType, new Dictionary<string, dynamic>(), parentEntities }));
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


                Intialize(autoContinue, machineInfo, processInfos, dbComplexAction, viewInfos);
            }

            
        }
    }
}
