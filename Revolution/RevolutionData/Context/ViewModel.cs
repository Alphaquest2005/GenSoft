using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class ViewModel
    {
        public class Commands
        {
            public static IStateCommand CreateViewModel => new StateCommand("CreateViewModel","Create View Model","ViewModel","Unknown", Events.ViewModelCreated);
            public static IStateCommand LoadViewModel => new StateCommand("LoadViewModel", "Load View Model", "ViewModel", "Unknown", Events.ViewModelLoaded);
            public static IStateCommand UnloadViewModel => new StateCommand("UnloadViewModel", "Unload View Model", "ViewModel", "Unknown", Events.ViewModelLoaded);
            
            public static IStateCommand Initialized => new StateCommand("InitializedViewModel", "Initialized View Model","ViewModel", "Unknown", Events.Initialized);
            public static IStateCommand ChangeCurrentEntity => new StateCommand("ChangeCurrentEntity", "Change Process CurrentEntity", "ViewModel", "Unknown", Events.CurrentEntityChanged);
        }
        public class Events
        {
            public static IStateEvent ViewModelCreated => new StateEvent("ViewModelCreated","View Model Created", "", "ViewModel", "Unknown");
            public static IStateEvent ViewModelLoaded => new StateEvent("ViewModelLoaded", "View Model Loaded", "", "ViewModel", "Unknown");
            public static IStateEvent ViewModelStateChanged => new StateEvent("ViewModelStateChanged", "View Model State Changed", "", "ViewModel", "Unknown");
            public static IStateEvent Initialized => new StateEvent("ViewModelInitialized", "View Model Initialized", "", "ViewModel", "Unknown");
            public static IStateEvent CurrentEntityChanged => new StateEvent("CurrentEntityChanged", "View Current Entity Changed", "View Current Entity Changed", "ViewModel", "Unknown");

        }
    }


}
