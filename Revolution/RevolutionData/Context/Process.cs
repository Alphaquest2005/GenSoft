using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class Process
    {
        public class Commands
        {
            public static IStateCommand CompleteProcess => new StateCommand( "CompleteProcess", "Create Process Completed Message", "Process", "Unknown",  Events.ProcessCompleted);
            public static IStateCommand StartProcess => new StateCommand("StartProcess", "Start Process", "Process", "Unknown", Events.ProcessStarted);
            public static IStateCommand CreateState => new StateCommand("CreateIntialState", "Create Intial State", "Process", "Unknown", Events.StateUpdated);
            public static IStateCommand UpdateState => new StateCommand("UpdateState", "Update State", "Process", "Unknown", Events.StateUpdated);
            public static IStateCommand Error => new StateCommand("UpdateState", "Update State", "Process", "Unknown", Events.Error);
            public static IStateCommand CreateLog => new StateCommand("CreateLog", "Create Process Log", "Process", "Unknown", Events.LogCreated);
            public static IStateCommand CreateComplexEventLog => new StateCommand("CreateComplexEventLog", "Create ComplexEvent Log", "Process", "Unknown", Events.ComplexEventLogCreated);
            public static IStateCommand PublishState => new StateCommand("RequestState", "Request Process State", "Process", "Unknown", Events.StatePublished);
            public static IStateCommand CleanUpProcess => new StateCommand("Cleanup Process", "Clean up Process", "Process", "Unknown", Events.ProcessCleanedUp);
            public static IStateCommand ChangeMainEntity => new StateCommand("ChangeMainEntity", "Change MainEntity", "Process", "Unknown");
            
        }

        public class Events
        {
            public static IStateEvent ProcessStarted => new StateEvent("ProcessStarted", "Process Started","", "Process", "Unknown", new StateCommand("StartProcess", "Start Process", "Process", "Unknown"));
            public static IStateEvent ProcessTimeOut => new StateEvent("TimeOut", "Process Timed Out", "", "Process", "Unknown");
            public static IStateEvent ProcessCompleted => new StateEvent("ProcessCompleted", "Process Completed", "", "Process", "Unknown");
            public static IStateEvent StateUpdated => new StateEvent("StateUpdated", "StateUpdated", "", "Process", "Unknown");
            public static IStateEvent LogCreated => new StateEvent("LogCreated", "Log Created", "", "Process", "Unknown");
            public static IStateEvent Error => new StateEvent("Error", "Log Created", "", "Process", "Unknown");
            public static IStateEvent ComplexEventLogCreated => new StateEvent("ComplexEventLogCreated", "ComplexEvent Log Created", "", "Process", "Unknown");
            public static IStateEvent StatePublished => new StateEvent("StatePublished", "Process State Published", "", "Process", "Unknown");
            public static IStateEvent ProcessCleanedUp => new StateEvent("ProcessCleanUp", "Process CleanUp", "", "Process", "Unknown", new StateCommand("CleanupProcess", "Cleanup Process", "Process", "Unknown"));
            
            public static IStateEvent CurrentApplicationChanged => new StateEvent("CurrentApplicationChanged", "Current Application Changed", "Current Application Changed", "Process", "Unknown");
            public static IStateEvent MainEntityChanged => new StateEvent("MainEntityChanged", "Process Main Entity Changed", "", "Process", "Unknown");
            //closed Loop
        }

    }
}
