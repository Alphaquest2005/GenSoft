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
using Common;
using Common.DataEntites;
using Common.Dynamic;
using DataServices.Actors;
using DynamicExpresso;
using GenSoft.DBContexts;
using GenSoft.Expressions;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Process.WorkFlow;
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;
using ViewModel.WorkFlow;

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

                var vmres = GetDBViewInfos();
                vmres.AddRange(viewInfos);

                System.ActorOf(Props.Create<ServiceManager>(autoRun,machineInfo, processInfos, res,vmres),"ServiceManager");
                Instance = this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IViewModelInfo> GetDBViewInfos()
        {
            var res = new List<IViewModelInfo>();
            using (var ctx = new GenSoftDBContext())
            {
                foreach (var r in ctx.DomainEntityType
                    .Include(x => x.DomainEntityTypeSourceEntity)
                    .Include(x => x.EntityType.Type)
                    .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.EntityType.DomainEntityType)
                    .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.ParentEntitys).ThenInclude(x => x.ParentEntity.EntityType.Type)
                    .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.ParentEntitys).ThenInclude(x => x.ParentEntity.Attributes)
                    .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.ChildEntitys).ThenInclude(x=> x.ChildEntity.EntityType.Type)
                    .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.ChildEntitys).ThenInclude(x => x.ChildEntity.Attributes)
                    .Include(x => x.EntityType.EntityView)
                    .Include(x => x.EntityType.DomainEntityType.DomainEntityCache)
                    .Include(x => x.EntityType.EntityList)
                    .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.ProcessState.Process)
                    .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.EntityTypeViewModel).ThenInclude(x => x.ViewModelTypes)
                     .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.EntityTypeViewModel).ThenInclude(x => x.EntityViewModelCommands).ThenInclude(x => x.ViewModelCommands.CommandTypes)
                    .Where(x => x.EntityType.EntityTypeAttributes.SelectMany(z => z.ParentEntitys).Any() || x.EntityType.EntityTypeAttributes.SelectMany(z => z.ChildEntitys).Any())
                    .OrderBy(x => x.Id)
                )
                {
                    
                    foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id)
                        .Distinct())
                    {
                        
                        foreach (var pd in r.ProcessStateDomainEntityTypes.Where(
                            x => x.ProcessState.ProcessId == processId).DistinctBy(x => x.Id))
                        {
                            foreach (var vm in pd.EntityTypeViewModel.DistinctBy(x => x.Id))
                            {
                                res.Add(ProcessViewModels.ProcessViewModelFactory[vm.ViewModelTypes.Name].Invoke(vm));
                            }
                        }

                    }
                }
            }
            return res;
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


                       if(! AddDynamicEntityTypes(ctx, entityType)) continue;
                        
                        foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            

                            if (r.EntityType.EntityView != null)
                            {
                                if (r.EntityType.EntityList == null)
                                {

                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateState", new object[] {processId,DynamicEntityType.DynamicEntityTypes[entityType]}));
                                }
                                else
                                {
                                    if(r.EntityType.EntityList.Initalize) res.Add(Processes.ComplexActions.GetComplexAction("IntializeProcessState", new object[] { processId, DynamicEntityType.DynamicEntityTypes[entityType] }));
                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList", new object[] {processId, DynamicEntityType.DynamicEntityTypes[entityType] }));
                                }
                            }

                            if(r.EntityType.DomainEntityType.DomainEntityCache != null )
                                res.Add(EntityComplexActions.GetComplexAction("IntializeCache",  new object[] { processId, DynamicEntityType.DynamicEntityTypes[entityType] }));

                            
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
                                                    EntityType = DynamicEntityType.DynamicEntityTypes[p.ParentEntity.EntityType.Type.Name],
                                                    Property = p.ChildEntity.Attributes.Name}
                                                    ).ToList();

                            res.Add(Processes.ComplexActions.GetComplexAction("RequestCompositeStateList",
                                new object[] { processId, DynamicEntityType.DynamicEntityTypes[childType], new Dictionary<string, dynamic>(), parentEntities }));
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

        private static bool AddDynamicEntityTypes(GenSoftDBContext ctx, string entityType)
        {
            var viewType = ctx.EntityView
                .FirstOrDefault(x => x.EntityType.Type.Name == entityType);
            if (viewType == null) return false;

            var viewset = ctx.EntityTypeAttributes
                .Where(x => x.EntityTypeId == viewType.Id)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => x.AttributeId).ToList();

            if (!viewset.Any()) return false;
            var tes = ctx.Entity
                          .FirstOrDefault(x => x.Id == 0 && x.EntityTypeId == viewType.BaseEntityTypeId)
                          ?.EntityAttribute
                          .Where(z => viewset.Contains(z.AttributeId))
                          .OrderBy(d => viewset.IndexOf(d.AttributeId))
                          .Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value,
                              z.Attributes.EntityId != null, z.Attributes.EntityName != null) as IEntityKeyValuePair)
                          .ToList() 
                   ?? ctx.EntityTypeAttributes
                          .Where(x => x.EntityTypeId == viewType.Id)
                          .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                          .Select(z => new EntityKeyValuePair(z.Attributes.Name, null,
                              z.Attributes.EntityId != null, z.Attributes.EntityName != null) as IEntityKeyValuePair).ToList();

            DynamicEntityType.DynamicEntityTypes.Add(entityType,
                new DynamicEntityType(entityType, tes));
            return true;
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
