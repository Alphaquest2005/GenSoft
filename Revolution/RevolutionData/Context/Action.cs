using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class Addin
    {
        public class Commands
        {
            public static IStateCommand StartAddin => new StateCommand("StartAction", "Action to be Started", "Action", "Unknown", Events.ActionStarted);
        }
        public class Events
        {
            public static IStateEvent ActionStarted => new StateEvent("ActionStarted", "Action Started", "", "Action", "Unknown");
        }
    }
}