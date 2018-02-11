using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class View
    {
        public class Commands
        {
           // public static IStateCommand CreateViewModel => new StateCommand("CreateViewModel","Create View Model", Events.ViewModelCreated);

            public static IStateCommand NavigateToView => new StateCommand("NavigateToView", "Navigate To View", "View", "Unknown");
            public static IStateCommand ChangeVisibility => new StateCommand("ChangeVisibility", "Change View Visibility", "View", "Unknown");
        }
        public class Events
        {
            public static IStateEvent ProcessStateLoaded => new StateEvent("ProcessStateLoaded", "Process State Loaded", "", "View", "Unknown");
            public static IStateEvent NavigatedToView => new StateEvent("NavigatedToView", "Navigated To View", "Navigated To View", "View", "Unknown");
            public static IStateEvent VisibilityChanged => new StateEvent("VisibilityChanged", "View Visibility Changed","", "View", "Unknown");



        }
    }


}
