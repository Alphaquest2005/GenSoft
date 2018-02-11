using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using DomainUtilities;
using EventMessages.Commands;
using RevolutionData;
using RevolutionData.Context;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace Process.WorkFlow
{

   

    public static class Processes
    {
        public static readonly ISystemProcess NullSystemProcess = new SystemProcess(new SystemProcessInfo(0, NullSystemProcess, "Null System Process", "Null System Process", "NullSystemProcess", "NulSystemProcess"), new Agent("System"), ThisMachineInfo);
        public static readonly ISystemProcess IntialSystemProcess = new SystemProcess(new SystemProcessInfo(1, NullSystemProcess, "Starting System", "Prepare system for Intial Use", "Start","System"),new Agent("System"), ThisMachineInfo);

        public static IMachineInfo ThisMachineInfo => new MachineInfo(Environment.MachineName, Environment.ProcessorCount);


        public static List<IComplexEventAction> ProcessComplexEvents = new List<IComplexEventAction>
        {

             new ComplexEventAction(
                "100",
                IntialSystemProcess, new List<IProcessExpectedEvent>
                {
                   // new ProcessExpectedEvent ("ServiceManagerStarted", 1, typeof (IServiceStarted<IServiceManager>), e => e != null, new StateEventInfo(1, RevolutionData.Context.Actor.Events.ActorStarted), new SourceType(typeof(IServiceManager))),
                    new ProcessExpectedEvent("ProcessServiceStarted", IntialSystemProcess, typeof(IServiceStarted<IProcessService>),e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),new SourceType(typeof(IProcessService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(IntialSystemProcess,RevolutionData.Context.Process.Commands.StartProcess ),
                action: ProcessActions.Actions["ProcessStarted"]),

             

            new ComplexEventAction(
                "102",
                IntialSystemProcess, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessStarted", IntialSystemProcess, typeof (ISystemProcessStarted), e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess, RevolutionData.Context.Process.Events.ProcessStarted, Guid.NewGuid()), new SourceType(typeof(IProcessService))),
                    new ProcessExpectedEvent ("ViewCreated", IntialSystemProcess, typeof (IViewModelCreated<IScreenModel>), e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess,"ScreenViewCreated", "ScreenView Created","This view contains all views", "ViewModel", "IScreenModel", RevolutionData.Context.ViewModel.Commands.CreateViewModel, Guid.NewGuid()), new SourceType(typeof(IViewModelService) )),
                    new ProcessExpectedEvent ("ViewLoaded", IntialSystemProcess, typeof (IViewModelLoaded<IMainWindowViewModel,IScreenModel>), e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess,"ScreenViewLoaded","ScreenView Model loaded in MainWindowViewModel","Only ViewModel in Body", "ViewModel", "IMainWindowViewModel,IScreenModel", RevolutionData.Context.ViewModel.Commands.LoadViewModel, Guid.NewGuid()), new SourceType(typeof(IViewModelService) ))
                },
                typeof(ISystemProcessCompleted),
                processInfo:new StateCommandInfo(IntialSystemProcess,RevolutionData.Context.Process.Commands.CompleteProcess ),
                action: ProcessActions.Actions["CompleteProcess"]),

           new ComplexEventAction(
                "103",
                IntialSystemProcess, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent ("ProcessCompleted", IntialSystemProcess, typeof (ISystemProcessCompleted), e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess, RevolutionData.Context.Process.Events.ProcessCompleted, Guid.NewGuid()), new SourceType(typeof(IComplexEventService))),

                },
                typeof(ISystemProcessStarted),
                processInfo:new StateCommandInfo(IntialSystemProcess,RevolutionData.Context.Process.Commands.StartProcess),
                action: ProcessActions.Actions["StartProcess"]),

            //new ComplexEventAction(
            //    "104",
            //    IntialSystemProcess, new List<IProcessExpectedEvent>
            //    {
            //        new ProcessExpectedEvent ("ProcessCompleted", IntialSystemProcess, typeof (ISystemProcessCompleted), e => e != null  && e.Process.Id == 1, new StateEventInfo(IntialSystemProcess, RevolutionData.Context.Process.Events.ProcessCompleted), new SourceType(typeof(IComplexEventService))),

            //    },
            //    typeof(ISystemProcessCleanedUp),
            //    processInfo:new StateCommandInfo(IntialSystemProcess,RevolutionData.Context.Process.Commands.CleanUpProcess ),
            //    action: ProcessActions.Actions["CleanUpProcess"]),

        };


        public static class ComplexActions
        {
            public static ComplexEventAction ProcessStarted(ISystemProcess process)
            {
                return new ComplexEventAction(
                    $"ProcessStarted-{process.Name}",
                    process, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent("ProcessServiceStarted", process, typeof(IServiceStarted<IProcessService>),
                            e => e != null && e.Process == process, new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                       
                    },
                    typeof(ISystemProcessStarted),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.StartProcess),
                    action: ProcessActions.Actions["ProcessStarted"]);
            }

            public static ComplexEventAction StartNextProcess(ISystemProcess process)
            {
                return new ComplexEventAction(
                    $"StartProcess-{process.Name}",
                    process, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent ("ProcessCompleted", process, typeof (ISystemProcessCompleted), e => e != null && e.Process == process, new StateEventInfo(process, RevolutionData.Context.Process.Events.ProcessCompleted, Guid.NewGuid()), new SourceType(typeof(IComplexEventService))),
                    },
                    typeof(ISystemProcessStarted),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.StartProcess),
                    action: ProcessActions.Actions["StartProcess"],
                    actionTrigger: ActionTrigger.Any);
            }

            public static ComplexEventAction CleanUpProcess(ISystemProcess process)
            {
                return new ComplexEventAction(
                    $"CleanUpProcess-{process.Name}",
                   
                    process, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent("CurrentApplicationChanged", process, typeof(ICurrentApplicationChanged),
                            e => e != null , new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                        new ProcessExpectedEvent("ProcessServiceStarted", process, typeof(IStartSystemProcess),
                            e => e != null && e.Process == process , new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                        new ProcessExpectedEvent("MainEntityChanged", process, typeof(IMainEntityChanged),
                            e => e != null , new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                        

                    },
                    typeof(ICleanUpSystemProcess),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.CleanUpProcess),
                    action: ProcessActions.Actions["CleanUpProcess"],
                    actionTrigger:ActionTrigger.Any);
            }

            public static ComplexEventAction CleanUpParentProcess(ISystemProcess process)
            {
                return new ComplexEventAction(
                    $"CleanUpParentProcess-{process.ParentProcess.Name}",
                    process.ParentProcess, new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent("StartSystemProcess", process.ParentProcess, typeof(IStartSystemProcess),
                            e => e != null , new StateEventInfo(process.ParentProcess, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                        new ProcessExpectedEvent("MainEntityChanged", process.ParentProcess, typeof(IMainEntityChanged),
                            e => e != null , new StateEventInfo(process.ParentProcess, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),
                        new ProcessExpectedEvent("CurrentApplicationChanged", process.ParentProcess, typeof(ICurrentApplicationChanged),
                            e => e != null , new StateEventInfo(process.ParentProcess, RevolutionData.Context.Actor.Events.ActorStarted, Guid.NewGuid()),
                            new SourceType(typeof(IProcessService))),

                    },
                    typeof(ICleanUpSystemProcess),
                    processInfo: new StateCommandInfo(process.ParentProcess, RevolutionData.Context.Process.Commands.CleanUpProcess),
                    action: ProcessActions.Actions["CleanUpParentProcess"],
                    actionTrigger: ActionTrigger.Any);
            }

            //public static ComplexEventAction IntializePulledProcessState(ISystemProcess process, string entityName,Type type)
            //{
            //    return (ComplexEventAction)typeof(ComplexActions).GetMethod("IntializePulledProcessState").MakeGenericMethod(type).Invoke(null, new object[] {process, entityName});
            //}
            public static ComplexEventAction IntializeProcessStateList(ISystemProcess process, IDynamicEntityType entityType)
            {
                return new ComplexEventAction(

                    key: $"InitalizeProcessState-{entityType.Name}",
                    process: process,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent(key: "ViewModelIntialized",
                            process: process,
                            eventPredicate: e => e != null && e.Process == process && ((IEntityViewInfo)((IViewModelIntialized)e)?.ViewModel.ViewInfo)?.EntityType == entityType  ,
                            eventType: typeof (IViewModelIntialized),
                            processInfo: new StateCommandInfo(process,RevolutionData.Context.CommandFunctions.UpdateCommandData(entityType.Name, RevolutionData.Context.ViewModel.Commands.Initialized), Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof (IComplexEventService))),
                        
                    },
                    expectedMessageType: typeof (IProcessStateMessage),
                    action: ProcessActions.IntializeProcessStateList(entityType),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.CreateState));
            }

            public static ComplexEventAction IntializeProcessState(ISystemProcess process, IDynamicEntityType entityType)
            {
                return new ComplexEventAction(

                    key: $"InitalizeProcessState-{entityType.Name}",
                    process: process,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent(key: "ViewModelIntialized",
                            process: process,
                            eventPredicate: e => e != null && e.Process == process && ((IEntityViewInfo)((IViewModelIntialized)e)?.ViewModel.ViewInfo)?.EntityType == entityType  ,
                            eventType: typeof (IViewModelIntialized),
                            processInfo: new StateEventInfo(process, RevolutionData.Context.Process.Events.ProcessStarted, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof (IComplexEventService))),

                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.IntializeProcessState(entityType),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.CreateState));
            }


            public static ComplexEventAction UpdateState(ISystemProcess process, IDynamicEntityType entityType)
            {
                return new ComplexEventAction(
                    key: $"UpdateState-{entityType.Name}",
                    process: process,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                                new ProcessExpectedEvent<IEntityWithChangesUpdated>(process: process,
                            eventPredicate: e => e.Entity != null && e.Process == process,
                            processInfo: new StateEventInfo(process, Entity.Events.EntityUpdated, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity"),
                                   new ProcessExpectedEvent<IEntityWithChangesFound>(process: process,
                            eventPredicate: e => e.Entity != null && e.Process == process,
                            processInfo: new StateEventInfo(process, Entity.Events.EntityFound, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity"),
                                   new ProcessExpectedEvent<IEntityFound>(process: process,
                            eventPredicate: e => e.Entity != null && e.Process == process,
                            processInfo: new StateEventInfo(process, Entity.Events.EntityFound, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof(IEntityRepository)),
                            key: "Entity")
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.UpdateEntityViewState(),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
            }

            
            public static ComplexEventAction RequestState(ISystemProcess process,string currentEntityType, string viewEntityType, string property) 
            {
                return new ComplexEventAction(
                    key: $"RequestState-{currentEntityType}-{viewEntityType}-{property}",
                    process: process,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged>(
                            "CurrentEntity", process, e => e.Entity != null && e.Process == process && e.Entity.Id > 0 && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(process, RevolutionData.Context.ViewModel.Events.CurrentEntityChanged, Guid.NewGuid())),

                        new ProcessExpectedEvent<IEntityFound>(
                            "CurrentEntity", process, e => e.Entity != null && e.Process == process && e.Entity.Id > 0 && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(process, Entity.Events.EntityFound, Guid.NewGuid())),
                        new ProcessExpectedEvent<IEntityUpdated>(
                            "CurrentEntity", process, e => e.Entity != null && e.Process == process && e.Entity.Id > 0  && e.Entity.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(process, Entity.Events.EntityUpdated, Guid.NewGuid())),
                        //new ProcessExpectedEvent<IEntityWithChangesFound>(
                        //    "CurrentEntity", process, e => e.Entity != null && e.Entity.Id > 0 && e.Entity.EntityType.Name == currentEntityType,
                        //    expectedSourceType: new SourceType(typeof (IViewModel)),
                        //    //todo: check this cuz it comes from viewmodel
                        //    processInfo: new StateEventInfo(process, Entity.Events.EntityFound))
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestState(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(viewEntityType), property),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
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

            public static ComplexEventAction UpdateStateList(ISystemProcess process, IDynamicEntityType entityType) 
            {
                return new ComplexEventAction(
                    key: $"UpdateStateList-{entityType.Name}",
                    process: process,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>
                    {
                            new ProcessExpectedEvent<IEntitySetWithChangesLoaded>(
                        "EntityViewSet",process, e => e.EntitySet != null && e.Process == process && e.EntityType == entityType, expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                        processInfo: new StateEventInfo(process, Entity.Events.EntitySetLoaded, Guid.NewGuid())),

                        new ProcessExpectedEvent<IEntitySetLoaded>(
                            "EntityViewSet",process, e => e.EntitySet != null && e.Process == process && e.EntityType == entityType, expectedSourceType: new SourceType(typeof(IEntityViewRepository)),
                            processInfo: new StateEventInfo(process, Entity.Events.EntitySetLoaded, Guid.NewGuid()))
                    },
                    expectedMessageType: typeof(IProcessStateList),
                    action: ProcessActions.UpdateEntityViewStateList(),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
            }
            public static ComplexEventAction RequestStateList(ISystemProcess process, string currentEntityType, string viewEntityType, string currentProperty, string viewProperty)
            {
                return new ComplexEventAction(
                    key: $"RequestStateList-{currentEntityType}-{viewEntityType}-{viewProperty}",
                    process: process,
                    actionTrigger: ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<ICurrentEntityChanged>(
                            "CurrentEntity", process, e => e.Entity != null && e.Process == process && e.Entity.Id > 0 && e.EntityType.Name == currentEntityType,
                            expectedSourceType: new SourceType(typeof (IViewModel)),
                            //todo: check this cuz it comes from viewmodel
                            processInfo: new StateEventInfo(process, RevolutionData.Context.ViewModel.Events.CurrentEntityChanged, Guid.NewGuid())),
                        
                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestStateList(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(viewEntityType),currentProperty,viewProperty),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IComplexEventAction UpdateStateWhenDataChanges(ISystemProcess process, string currentEntityType, string viewEntityType, string currentProperty, string viewProperty)
            {
                return new ComplexEventAction(
                    key: $"UpdateStateWhenDataChanges{currentEntityType}-{viewEntityType}-{viewProperty}",
                    process: process,
                    actionTrigger:ActionTrigger.Any, 
                    events: new List<IProcessExpectedEvent>
                    {
                        new ProcessExpectedEvent<IEntityUpdated>(process: process,
                            eventPredicate: e => e.Entity != null && e.Process == process && e.EntityType.Name == currentEntityType,
                            processInfo: new StateEventInfo(process, Entity.Events.EntityUpdated, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),
                        new ProcessExpectedEvent<IEntityWithChangesUpdated>(process: process,
                            eventPredicate: e => e.Entity != null && e.Process == process && e.EntityType.Name == currentEntityType,
                            processInfo: new StateEventInfo(process, Entity.Events.EntityUpdated, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof (IEntityRepository)),
                            key: "UpdatedEntity"),


                    },
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: GetView(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(viewEntityType),currentProperty, viewProperty),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessAction GetView(IDynamicEntityType entityType, string currentProperty, string viewProperty) 
            {
                return new ProcessAction(
                    action:
                        async cp =>
                        {
                            var key = viewProperty;
                            var value = cp.Messages["UpdatedEntity"].Properties["Entity"].GetValue<IDynamicEntity>().Properties[currentProperty];
                            var changes = new Dictionary<string, dynamic>() { { key, value } };

                            return await Task.Run(() => new GetEntitySetWithChanges("ExactMatch",entityType,changes,
                                new StateCommandInfo(cp.Actor.Process, Entity.Commands.GetEntity),
                                cp.Actor.Process, cp.Actor.Source)).ConfigureAwait(false);
                        },
                    processInfo: cp =>
                        new StateCommandInfo(cp.Actor.Process,
                            Entity.Commands.GetEntity),
                    // take shortcut cud be IntialState
                    expectedSourceType: new SourceType(typeof(IComplexEventService))

                    );
            }


            public static ComplexEventAction RequestCompositeStateList(ISystemProcess process,IDynamicEntityType entityType, Dictionary<string, dynamic>changes, List<ViewModelEntity> entities)
            {
                
                return new ComplexEventAction(
                    key: $"RequestCompositState-{string.Join(",", entities.Select(x => x.EntityType))}",
                    process: process,
                    actionTrigger: ActionTrigger.Any,
                    events: new List<IProcessExpectedEvent>(entities.Select(x => CreateProcessCurrentEntityChangedExpectedEvent(process, entityType)).ToList()),
                                       
                    expectedMessageType: typeof(IProcessStateMessage),
                    action: ProcessActions.RequestCompositStateList(entityType, changes, entities),
                    processInfo: new StateCommandInfo(process, RevolutionData.Context.Process.Commands.UpdateState));
            }

            public static IProcessExpectedEvent CreateProcessCurrentEntityChangedExpectedEvent(ISystemProcess process, IDynamicEntityType entityType)
            {
               return new ProcessExpectedEvent<ICurrentEntityChanged>(process: process,
                            eventPredicate: e => e.Entity != null && e.Entity.Id > 0 &&  e.EntityType == entityType,
                            processInfo: new StateEventInfo(process, RevolutionData.Context.ViewModel.Events.CurrentEntityChanged, Guid.NewGuid()),
                            expectedSourceType: new SourceType(typeof(IComplexEventService)),
                            key: $"CurrentEntity-{entityType.Name}");
            }


           

            private static IProcessAction CreateProcessAction(object action)
            {
                throw new NotImplementedException();
            }

            private static IProcessExpectedEvent CreateProcessExpectedEvent( object o)
            {
                throw new NotImplementedException();
            }
        }

        
    }

    
}

