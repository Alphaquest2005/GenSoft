using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class ViewModel
    {
        public class Commands
        {
            public static IStateCommand CreateViewModel => new StateCommand("CreateViewModel","Create View Model", Events.ViewModelCreated);
            public static IStateCommand LoadViewModel => new StateCommand("LoadViewModel", "Load View Model", Events.ViewModelLoaded);
            public static IStateCommand UnloadViewModel => new StateCommand("UnloadViewModel", "Unload View Model", Events.ViewModelLoaded);
            public static IStateCommand ChangeVisibility => new StateCommand("ChangeVisibility", "Change View Model Visibility");
        }
        public class Events
        {
            public static IStateEvent ViewModelCreated => new StateEvent("ViewModelCreated","View Model Created", "");
            public static IStateEvent ViewModelLoaded => new StateEvent("ViewModelLoaded", "View Model Loaded", "");
            public static IStateEvent ViewModelStateChanged => new StateEvent("ViewModelStateChanged", "View Model State Changed", "");
        }
    }


}
