using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using BuildingSpecifications;
using Common.Dynamic;
using DataServices.Actors;
using GenSoft.DBContexts;
using GenSoft.Expressions;
using Microsoft.EntityFrameworkCore;
using Process.WorkFlow;
using RevolutionEntities.Process;
using ViewModel.Interfaces;


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
            //ComplexActions.GetComplexAction("RequestPulledState", new[] { typeof(IPatientInfo), typeof(IPatientDetailsInfo) }, new object[] { 3, "Patient", }),
            var res = new List<IComplexEventAction>();
            using (var ctx = new GenSoftDBContext())
            {
                foreach (var r in ctx.EntityRelationships
                                     .Include(x => x.ChildEntity.EntityTypeAttributes.EntityType.Type)
                                     .Include(x => x.ParentEntity.EntityTypeAttributes.EntityType.Type)
                                     .Include(x => x.ChildEntity.EntityTypeAttributes.EntityType.DomainEntityType)
                                     .Include("ChildEntity.EntityTypeAttributes.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process"))
                {
                    var parentType = interfaces.GetTypes().FirstOrDefault(x => x.Name == r.ParentEntity.EntityTypeAttributes.EntityType.Type.Name);
                    var childType = interfaces.GetTypes().FirstOrDefault(x => x.Name == r.ChildEntity.EntityTypeAttributes.EntityType.Type.Name);
                    var sourceEntityName = r.ChildEntity.EntityTypeAttributes.EntityType.DomainEntityType.SourceEntity;
                    foreach (var processId in r.ChildEntity.EntityTypeAttributes.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                    {
                        res.Add(Processes.ComplexActions.GetComplexAction("RequestPulledState",
                            new[] {parentType, childType}, new object[] {processId, sourceEntityName}));
                    }
                    
                }
            }
            return res;
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
