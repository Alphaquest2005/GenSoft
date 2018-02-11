using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class Actor
    {
        public class Commands
        {
            public static IStateCommand CreateAction => new StateCommand("CreateAction", "Action to be Executed", "Actor", "Unknown", Events.ActionCreated);
            public static IStateCommand StartActor => new StateCommand("StartService", "Start Actor Service", "Actor", "Unknown", Events.ActorStarted);
            public static IStateCommand StopActor => new StateCommand("ShutActorDown", "Shut Actor Down", "Actor", "Unknown", Events.ActorStopped);
            public static IStateCommand CreateActor => new StateCommand("CreateService", "Create Actor Service", "Actor", "Unknown", Events.ActionCreated);

        }
        public class Events
        {
            public static IStateEvent ActorStarted => new StateEvent("ServiceStarted", "Service Started", "", "Actor", "Unknown");
            public static IStateEvent ActorStopped => new StateEvent("ActorShutDown", "Actor Terminated", "", "Actor", "Unknown");
            public static IStateEvent ActionCreated => new StateEvent("ActionCreated", "Action Created", "", "Actor", "Unknown");
        }

    }
    
}
