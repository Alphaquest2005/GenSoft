using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class Entity
    {
        public class Commands
        {
            public static IStateCommand CreateEntity => new StateCommand("CreateEntity", "Create Entity", "Entity", "Unknown", Events.EntityCreated);
            public static IStateCommand UpdateEntity => new StateCommand("UpdateEntity", "Update Entity", "Entity", "Unknown", Events.EntityCreated);
            public static IStateCommand GetEntity => new StateCommand("GetEntity", "Get Entity", "Entity", "Unknown", Events.EntityCreated);

            public static IStateCommand EntityRequest => new StateCommand("EntityRequest", "Entity Request", "Entity", "Unknown", Events.EntityRequested);
            public static IStateCommand DeleteEntity => new StateCommand("DeleteEntity", "Delete Entity", "Entity", "Unknown", Events.EntityCreated);
            public static IStateCommand LoadEntitySet => new StateCommand("LoadEntitySet", "Load Entity Set", "Entity", "Unknown", Events.EntityCreated);
            public static IStateCommand LoadEntitySetWithChanges => new StateCommand("LoadEntitySetWithChanges", "Load EntitySet with Changes", "Entity", "Unknown", Events.EntitySetLoaded);

            public static IStateCommand InitializeState => new StateCommand("CreateIntialState", "Create Intial State", "Entity", "Unknown", Events.StateUpdated);
            public static IStateCommand UpdateState => new StateCommand("UpdateState", "Update State", "Entity", "Unknown", Events.StateUpdated);
            public static IStateCommand PublishState => new StateCommand("RequestState", "Request Process State", "Entity", "Unknown", Events.StatePublished);
        }

        public class Events
        {
            public static IStateEvent EntityCreated => new StateEvent("EntityCreated", "Entity Created", "", "Entity", "Unknown");
            public static IStateEvent EntityUpdated => new StateEvent("EntityUpdated", "Entity Updated", "", "Entity", "Unknown");
            public static IStateEvent EntityDeleted => new StateEvent("EntityDeleted", "Entity Deleted", "", "Entity", "Unknown");
            public static IStateEvent EntityFound => new StateEvent("EntityFound", "Entity Found", "", "Entity", "Unknown");
            public static IStateEvent EntitySetLoaded => new StateEvent("EntitySetLoaded", "Entity Set Loaded", "", "Entity", "Unknown");
            public static IStateEvent EntityRequested => new StateEvent("EntityRequested", "Entity Requested", "", "Entity", "Unknown");

            public static IStateEvent StateUpdated => new StateEvent("StateUpdated", "StateUpdated", "", "Entity", "Unknown");
            public static IStateEvent StatePublished => new StateEvent("StatePublished", "Process State Published", "", "Entity", "Unknown");
        }
    }
}