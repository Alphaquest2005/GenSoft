﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using Domain.Interfaces;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionData;
using RevolutionData.Context;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;

namespace Process.WorkFlow
{

   

    public static class Processes
    {
        public static readonly List<IProcessInfo> ProcessInfos = new List<IProcessInfo>
        {
            //new Process(0,0, "Uknown Process", "Unknown Process", "Unknown"),
            new ProcessInfo(1, 0, "Starting System", "Prepare system for Intial Use", "Start","System"),
            new ProcessInfo(2, 1, "User SignOn", "User Login", "User","System"),
            new ProcessInfo(3, 2, "Load User Screen", "User Screen", "UserScreen", "joe")
        };




        public static List<IComplexEventAction> ProcessComplexEvents = new List<IComplexEventAction>
        {

             new ComplexEventAction(
                "100",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ServiceManagerStarted", 1, typeof (IServiceStarted<IServiceManager>), e => e != null, new StateEventInfo(1, RevolutionData.Context.Actor.Events.ActorStarted), new SourceType(typeof(IServiceManager))),
                    
                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.StartProcess ),
                action: ProcessActions.Actions["ProcessStarted"]),

             

            new ComplexEventAction(
                "102",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessStarted", 1, typeof (ISystemProcessStarted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessStarted), new SourceType(typeof(IProcessService))),
                    new ProcessExpectedEvent ("ViewCreated", 1, typeof (IViewModelCreated<IScreenModel>), e => e != null, new StateEventInfo(1,"ScreenViewCreated", "ScreenView Created","This view contains all views", RevolutionData.Context.ViewModel.Commands.CreateViewModel), new SourceType(typeof(IViewModelService) )),
                    new ProcessExpectedEvent ("ViewLoaded", 1, typeof (IViewModelLoaded<IMainWindowViewModel,IScreenModel>), e => e != null, new StateEventInfo(1,"ScreenViewLoaded","ScreenView Model loaded in MainWindowViewModel","Only ViewModel in Body", RevolutionData.Context.ViewModel.Commands.LoadViewModel), new SourceType(typeof(IViewModelService) ))
                },
                typeof(ISystemProcessCompleted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.CompleteProcess ),
                action: ProcessActions.Actions["CompleteProcess"]),

           new ComplexEventAction(
                "103",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.StartProcess),
                action: ProcessActions.Actions["StartProcess"]),

            new ComplexEventAction(
                "104",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessCleanedUp),
                processInfo:new StateCommandInfo(1,RevolutionData.Context.Process.Commands.CleanUpProcess ),
                action: ProcessActions.Actions["CleanUpProcess"]),

            
            //new ComplexEventAction(
            //    "106",
            //    1, new List<IProcessExpectedEvent>
            //    {
            //        new ProcessExpectedEvent ("ProcessEventError", 1, typeof (IProcessEventFailure), e => e != null, new StateEventInfo(1, Context.Process.Events.Error), new SourceType(typeof(IComplexEventService))),

            //    },
            //    typeof(IProcessEventFailure),
            //    processInfo:new StateCommandInfo(1,Context.Process.Commands.Error ),
            //    action: ProcessActions.ShutDownApplication),

            ComplexActions.StartProcess(2),
            new ComplexEventAction(
                
                key:"201",
                processId:2,
                events:new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent (key: "ProcessStarted",
                                              processId: 2,
                                              eventPredicate: e => e != null,
                                              eventType: typeof (ISystemProcessStarted),
                                              processInfo: new StateEventInfo(2,RevolutionData.Context.Process.Events.ProcessStarted),
                                              expectedSourceType:new SourceType(typeof(IComplexEventService)))
                    
                },
                expectedMessageType:typeof(IProcessStateMessage),
                action:ProcessActions.SignIn.IntializeSigninProcessState,
                processInfo:new StateCommandInfo(2, RevolutionData.Context.Process.Commands.CreateState)),
            new ComplexEventAction(
                key:"202",
                processId:2,
                events:new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityWithChangesFound> (
                        "UserNameFound", 2, e => e.Entity != null && e.Changes.Count == 1 && e.Changes.ContainsKey("Usersignin"), expectedSourceType: new SourceType(typeof(IEntityViewRepository)), processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserNameFound))
                },
                expectedMessageType:typeof(IProcessStateMessage),
                action: ProcessActions.SignIn.UserNameFound,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key:"203",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityWithChangesFound> (processId: 2,
                                                        eventPredicate: e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey("Password"),
                                                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                                                        expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                                                        key: "ValidatedUser")
                },
                expectedMessageType: typeof(IProcessStateMessage),
                action: ProcessActions.SignIn.SetProcessStatetoValidatedUser,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key:"204",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityWithChangesFound> (processId: 2,
                                                        eventPredicate: e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey("Password"),
                                                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                                                        expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                                                        key: "ValidatedUser")
                },
                expectedMessageType:typeof(IUserValidated),
                processInfo:new StateCommandInfo(2, RevolutionData.Context.Domain.Commands.PublishDomainEvent),
                action: ProcessActions.SignIn.UserValidated),

            new ComplexEventAction(
                "205",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ValidatedUser", 2, typeof (IUserValidated), e => e != null, new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound), new SourceType(typeof(IComplexEventService))),
                    
                },
                typeof(ISystemProcessCompleted),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.CompleteProcess ),
                action: ProcessActions.Actions["CompleteProcess"]),

             new ComplexEventAction(
                "206",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ValidatedUser", 2, typeof (IUserValidated), e => e != null, new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.StartProcess ),
                action: ProcessActions.Actions["StartProcessWithValidatedUser"]),

            new ComplexEventAction(
                "207",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", 2, typeof (ISystemProcessCompleted), e => e != null, new StateEventInfo(2, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessCleanedUp),
                processInfo:new StateCommandInfo(2,RevolutionData.Context.Process.Commands.CleanUpProcess ),
                action: ProcessActions.Actions["CleanUpProcess"]),

            
            ComplexActions.GetComplexAction("StartProcess", new object[]{3}),



           // ComplexActions.GetComplexAction("IntializeProcessState", new object[]{3, DynamicEntityType.DynamicEntityTypes["IPatientInfo"]}),

        };


        public static class ComplexActions
        {
            public static ComplexEventAction StartProcess(int processId)
            {
                return new ComplexEventAction(
                    $"StartProcess-{processId}",
                    processId, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent("ProcessServiceStarted", processId, typeof(IServiceStarted<IProcessService>),
                            e => e != null, new StateEventInfo(processId, RevolutionData.Context.Actor.Events.ActorStarted),
                            new SourceType(typeof(IProcessService))),

                    },
                    typeof(ISystemProcessStarted),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.StartProcess),
                    action: ProcessActions.Actions["ProcessStarted"]);
            }

            //public static ComplexEventAction IntializePulledProcessState(int processId, string entityName,Type type)
            //{
            //    return (ComplexEventAction)typeof(ComplexActions).GetMethod("IntializePulledProcessState").MakeGenericMethod(type).Invoke(null, new object[] {processId, entityName});
            //}
            public static ComplexEventAction IntializeProcessState(int processId, IDynamicEntityType entityType)
            {
                return new ComplexEventAction(

                    key: $"InitalizeProcessState-{entityType.Name}",
                    processId: processId,
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent(key: "ProcessStarted",
                            processId: processId,
                            eventPredicate: e => e != null,
                            eventType: typeof (ISystemProcessStarted),
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.ProcessStarted),
                            expectedSourceType: new SourceType(typeof (IComplexEventService))),
                        
                    },
                    expectedMessageType: typeof (IProcessStateMessage),
                    action: ProcessActions.IntializeProcessState(entityType),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.CreateState));
            }


            public static ComplexEventAction UpdateState(int processId, IDynamicEntityType entityType)
            {
                return new ComplexEventAction(
                    key: $"UpdateState-{entityType.Name}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                                new ProcessExpectedEvent<IEntityWithChangesUpdated>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity"),
                                   new ProcessExpectedEvent<IEntityWithChangesFound>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity"),
                                   new ProcessExpectedEvent<IEntityFound>(processId: processId,
                            eventPredicate: e => e.Entity != null,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity")
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.UpdateEntityViewState(entityType),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            
            public static ComplexEventAction RequestState(int processId,string currentEntityType, string viewEntityType, string property) 
            {
                return new ComplexEventAction(
                    key: $"RequestState-{currentEntityType}-{viewEntityType}-{property}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged>(
                            "CurrentEntity", processId, e => e.Entity != null && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged)),

                        new ProcessExpectedEvent<IEntityFound>(
                            "CurrentEntity", processId, e => e.Entity != null && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound)),
                        new ProcessExpectedEvent<IEntityUpdated>(
                            "CurrentEntity", processId, e => e.Entity != null && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated)),
                        new ProcessExpectedEvent<IEntityWithChangesFound>(
                            "CurrentEntity", processId, e => e.Entity != null && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityFound))
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestState(DynamicEntityType.DynamicEntityTypes[viewEntityType],property),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }



            public static ComplexEventAction GetComplexAction(string method, object[] args)
            {
                try
                {
                    return (ComplexEventAction) typeof(ComplexActions).GetMethod(method).Invoke(null, args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

            public static ComplexEventAction UpdateStateList(int processId, IDynamicEntityType entityType) 
            {
                return new ComplexEventAction(
                    key: $"UpdateStateList-{entityType.Name}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>
                    {
                            new ProcessExpectedEvent<IEntitySetWithChangesLoaded>(
                        "EntityViewSet",processId, e => e.EntitySet != null && e.EntityType == entityType, expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                        processInfo: new StateEventInfo(processId, Entity.Events.EntitySetLoaded)),

                        new ProcessExpectedEvent<IEntitySetLoaded>(
                            "EntityViewSet",processId, e => e.EntitySet != null && e.EntityType == entityType, expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                            processInfo: new StateEventInfo(processId, Entity.Events.EntitySetLoaded))
                    },
                    expectedMessageType: typeof(IProcessStateList),
                    action: ProcessActions.UpdateEntityViewStateList(entityType),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }
            public static ComplexEventAction RequestStateList(int processId, string currentEntityType, string viewEntityType, string currentProperty, string viewProperty)
            {
                return new ComplexEventAction(
                    key: $"RequestStateList-{currentEntityType}-{viewEntityType}-{viewProperty}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged>(
                            "CurrentEntity", processId, e => e.Entity != null && e.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged)),
                        
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestStateList(DynamicEntityType.DynamicEntityTypes[viewEntityType],currentProperty,viewProperty),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IComplexEventAction UpdateStateWhenDataChanges(int processId, string currentEntityType, string viewEntityType, string currentProperty, string viewProperty)
            {
                return new ComplexEventAction(
                    key: $"UpdateStateWhenDataChanges{currentEntityType}-{viewEntityType}-{viewProperty}",
                    processId: 3,
                    actionTrigger:ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<IEntityUpdated>(processId: processId,
                            eventPredicate: e => e.Entity != null && e.EntityType.Name == currentEntityType,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),
                        new ProcessExpectedEvent<IEntityWithChangesUpdated>(processId: processId,
                            eventPredicate: e => e.Entity != null && e.EntityType.Name == currentEntityType,
                            processInfo: new StateEventInfo(processId, Entity.Events.EntityUpdated),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),


                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: GetView(DynamicEntityType.DynamicEntityTypes[viewEntityType],currentProperty, viewProperty),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessAction GetView(IDynamicEntityType entityType, string currentProperty, string viewProperty) 
            {
                return new ProcessAction(
                    action:
                        async cp =>
                        {
                            var key = viewProperty;
                            var value = cp.Messages["UpdatedEntity"].Entity.Properties[currentProperty];
                            var changes = new Dictionary<string, dynamic>() { { key, value } };

                            return await Task.Run(() => new GetEntitySetWithChanges("ExactMatch",entityType,changes,
                                new StateCommandInfo(cp.Actor.Process.Id, Entity.Commands.GetEntity),
                                cp.Actor.Process, cp.Actor.Source));
                        },
                    processInfo: cp =>
                        new StateCommandInfo(cp.Actor.Process.Id,
                            Entity.Commands.GetEntity),
                    // take shortcut cud be IntialState
                    expectedSourceType: new SourceType(typeof(IComplexEventService))

                    );
            }


            public static ComplexEventAction RequestCompositeStateList(int processId,IDynamicEntityType entityType, Dictionary<string, dynamic>changes, List<ViewModelEntity> entities)
            {
                
                return new ComplexEventAction(
                    key: $"RequestCompositState-{string.Join(",", entities.Select(x => x.EntityType))}",
                    processId: processId,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>(entities.Select(x => CreateProcessExpectedEvent(processId, entityType)).ToList()),
                                       
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestCompositStateList(entityType, changes, entities),
                    processInfo: new StateCommandInfo(processId, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessExpectedEvent CreateProcessExpectedEvent(int processId, IDynamicEntityType entityType)
            {
               return new ProcessExpectedEvent<ICurrentEntityChanged>(processId: processId,
                            eventPredicate: e => e.Entity != null && e.EntityType == entityType,
                            processInfo: new StateEventInfo(processId, RevolutionData.Context.Process.Events.CurrentEntityChanged),
                            expectedSourceType: new SourceType(typeof(IComplexEventService)),
                            key: $"CurrentEntity-{entityType.Name}");
            }
        }
    }

    
}

