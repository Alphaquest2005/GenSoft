using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class Entity
    {
        public class Commands
        {
            public static IStateCommand CreateEntity => new StateCommand("CreateEntity", "Create Entity",Events.EntityCreated);
            public static IStateCommand UpdateEntity => new StateCommand("UpdateEntity", "Update Entity", Events.EntityCreated);
            public static IStateCommand GetEntity => new StateCommand("GetEntity", "Get Entity", Events.EntityCreated);

            public static IStateCommand EntityRequest => new StateCommand("EntityRequest", "Entity Request", Events.EntityRequested);
            public static IStateCommand DeleteEntity => new StateCommand("DeleteEntity", "Delete Entity", Events.EntityCreated);
            public static IStateCommand LoadEntitySet => new StateCommand("LoadEntitySet", "Load Entity Set", Events.EntityCreated);
            public static IStateCommand LoadEntitySetWithChanges => new StateCommand("LoadEntitySetWithChanges", "Load EntitySet with Changes", Events.EntitySetLoaded);
        }

        public class Events
        {
            public static IStateEvent EntityCreated => new StateEvent("EntityCreated", "Entity Created", "");
            public static IStateEvent EntityUpdated => new StateEvent("EntityUpdated", "Entity Updated", "");
            public static IStateEvent EntityDeleted => new StateEvent("EntityDeleted", "Entity Deleted", "");
            public static IStateEvent EntityFound => new StateEvent("EntityFound", "Entity Found", "");
            public static IStateEvent EntitySetLoaded => new StateEvent("EntitySetLoaded", "Entity Set Loaded", "");
            public static IStateEvent EntityRequested => new StateEvent("EntityRequested", "Entity Requested", "");
        }
    }
}