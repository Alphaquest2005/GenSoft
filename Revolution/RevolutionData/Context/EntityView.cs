//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SystemInterfaces;
//using RevolutionEntities.Process;

//namespace RevolutionData.Context
//{
//    public class EntityView
//    {
//        public class Commands
//        {
//            public static IStateCommand GetEntity => new StateCommand("GetEntity", "Get Entity View Item", Events.EntityViewFound );
//            public static IStateCommand LoadEntitySetWithChanges = new StateCommand("LoadEntitySetWithChanges", "Load EntityView Set with Changes", Events.EntitySetLoaded );
//        }

//        public class Events
//        {
//            public static IStateEvent EntityViewFound => new StateEvent("EntityViewFound", "Entity View Item Found", "");

//            public static IStateEvent EntitySetLoaded => new StateEvent("EntitySetLoaded", "Entity ViewSet Loaded", "");
//            public static IStateEvent EntityUpdated => new StateEvent("EntityUpdated", "Entity View Updated", "");
//        }
//    }
//}
