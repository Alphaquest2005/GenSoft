using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Domain.Interfaces;
using Interfaces;
using RevolutionEntities.Process;
using ViewModel.Interfaces;

namespace RevolutionData
{
    public static class StartUpProcess
    {
        public static List<IComplexEventAction> StartUpProcesses = new List<IComplexEventAction>
        {

            new ComplexEventAction(
                "100",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ServiceManagerStarted", 1, typeof (IServiceStarted<IServiceManager>),
                        e => e != null, new StateEventInfo(1, RevolutionData.Context.Actor.Events.ActorStarted),
                        new SourceType(typeof (IServiceManager))),

                },
                typeof (ISystemProcessStarted),
                processInfo: new StateCommandInfo(1, RevolutionData.Context.Process.Commands.StartProcess),
                action:  ProcessActions.Actions["ProcessStarted"]),



            new ComplexEventAction(
                "102",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ProcessStarted", 1, typeof (ISystemProcessStarted), e => e != null,
                        new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessStarted),
                        new SourceType(typeof (IProcessService))),
                    new ProcessExpectedEvent("ViewCreated", 1, typeof (IViewModelCreated<IScreenModel>), e => e != null,
                        new StateEventInfo(1, "ScreenViewCreated", "ScreenView Created", "This view contains all views",
                            RevolutionData.Context.ViewModel.Commands.CreateViewModel),
                        new SourceType(typeof (IViewModelService))),
                    new ProcessExpectedEvent("ViewLoaded", 1,
                        typeof (IViewModelLoaded<IMainWindowViewModel, IScreenModel>), e => e != null,
                        new StateEventInfo(1, "ScreenViewLoaded", "ScreenView Model loaded in MainWindowViewModel",
                            "Only ViewModel in Body", RevolutionData.Context.ViewModel.Commands.LoadViewModel),
                        new SourceType(typeof (IViewModelService)))
                },
                typeof (ISystemProcessCompleted),
                processInfo: new StateCommandInfo(1, RevolutionData.Context.Process.Commands.CompleteProcess),
                action: ProcessActions.Actions["CompleteProcess"]),

            new ComplexEventAction(
                "103",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null,
                        new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted),
                        new SourceType(typeof (IComplexEventService))),

                },
                typeof (ISystemProcessStarted),
                processInfo: new StateCommandInfo(1, RevolutionData.Context.Process.Commands.StartProcess),
                action: ProcessActions.Actions["StartProcess"]),

            new ComplexEventAction(
                "104",
                1, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ProcessCompleted", 1, typeof (ISystemProcessCompleted), e => e != null,
                        new StateEventInfo(1, RevolutionData.Context.Process.Events.ProcessCompleted),
                        new SourceType(typeof (IComplexEventService))),

                },
                typeof (ISystemProcessCleanedUp),
                processInfo: new StateCommandInfo(1, RevolutionData.Context.Process.Commands.CleanUpProcess),
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

            new ComplexEventAction(
                "200",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ProcessServiceStarted", 2, typeof (IServiceStarted<IProcessService>),
                        e => e != null, new StateEventInfo(2, RevolutionData.Context.Actor.Events.ActorStarted),
                        new SourceType(typeof (IProcessService))),

                },
                typeof (ISystemProcessStarted),
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.StartProcess),
                action:  ProcessActions.Actions["ProcessStarted"]),
            new ComplexEventAction(

                key: "201",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent(key: "ProcessStarted",
                        processId: 2,
                        eventPredicate: e => e != null,
                        eventType: typeof (ISystemProcessStarted),
                        processInfo: new StateEventInfo(2, RevolutionData.Context.Process.Events.ProcessStarted),
                        expectedSourceType: new SourceType(typeof (IComplexEventService)))

                },
                expectedMessageType: typeof (IProcessStateMessage<ISignInInfo>),
                action: ProcessActions.SignIn.IntializeSigninProcessState,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.CreateState)),
            new ComplexEventAction(
                key: "202",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>>(
                        "UserNameFound", 2,
                        e =>
                            e.Entity != null && e.Changes.Count == 1 &&
                            e.Changes.ContainsKey(nameof(ISignInInfo.Usersignin)),
                        expectedSourceType: new SourceType(typeof (IEntityViewRepository)),
                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserNameFound))
                },
                expectedMessageType: typeof (IProcessStateMessage<ISignInInfo>),
                action: ProcessActions.SignIn.UserNameFound,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key: "203",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>>(processId: 2,
                        eventPredicate:
                            e =>
                                e.Entity != null && e.Changes.Count == 2 &&
                                e.Changes.ContainsKey(nameof(ISignInInfo.Password)),
                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                        expectedSourceType: new SourceType(typeof (IEntityViewRepository)),
                        key: "ValidatedUser")
                },
                expectedMessageType: typeof (IProcessStateMessage<ISignInInfo>),
                action: ProcessActions.SignIn.SetProcessStatetoValidatedUser,
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.UpdateState)),
            new ComplexEventAction(
                key: "204",
                processId: 2,
                events: new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent<IEntityViewWithChangesFound<ISignInInfo>>(processId: 2,
                        eventPredicate:
                            e =>
                                e.Entity != null && e.Changes.Count == 2 &&
                                e.Changes.ContainsKey(nameof(ISignInInfo.Password)),
                        processInfo: new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                        expectedSourceType: new SourceType(typeof (IEntityViewRepository)),
                        key: "ValidatedUser")
                },
                expectedMessageType: typeof (IUserValidated),
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Domain.Commands.PublishDomainEvent),
                action: ProcessActions.SignIn.UserValidated),

            new ComplexEventAction(
                "205",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ValidatedUser", 2, typeof (IUserValidated), e => e != null,
                        new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                        new SourceType(typeof (IComplexEventService))),

                },
                typeof (ISystemProcessCompleted),
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.CompleteProcess),
                action: ProcessActions.Actions["CompleteProcess"]),

            new ComplexEventAction(
                "206",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ValidatedUser", 2, typeof (IUserValidated), e => e != null,
                        new StateEventInfo(2, RevolutionData.Context.User.Events.UserFound),
                        new SourceType(typeof (IComplexEventService))),

                },
                typeof (ISystemProcessStarted),
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.StartProcess),
                action: ProcessActions.Actions["StartProcessWithValidatedUser"]),

            new ComplexEventAction(
                "207",
                2, new List<IProcessExpectedEvent>
                {
                    new ProcessExpectedEvent("ProcessCompleted", 2, typeof (ISystemProcessCompleted), e => e != null,
                        new StateEventInfo(2, RevolutionData.Context.Process.Events.ProcessCompleted),
                        new SourceType(typeof (IComplexEventService))),

                },
                typeof (ISystemProcessCleanedUp),
                processInfo: new StateCommandInfo(2, RevolutionData.Context.Process.Commands.CleanUpProcess),
                action: ProcessActions.Actions["CleanUpProcess"]),
        };
    }
}
