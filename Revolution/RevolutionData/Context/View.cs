﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public class View
    {
        public class Commands
        {
           // public static IStateCommand CreateViewModel => new StateCommand("CreateViewModel","Create View Model", Events.ViewModelCreated);

            public static IStateCommand ChangeEntity => new StateCommand("ChangeEntity", "Change Entity");
            public static IStateCommand NavigateToView => new StateCommand("NavigateToView", "Navigate To View");
        }
        public class Events
        {
            public static IStateEvent ProcessStateLoaded => new StateEvent("ProcessStateLoaded", "Process State Loaded", "");
            public static IStateEvent EntityChanged => new StateEvent("EntityChanged", "Entity Changed", "Changes from User Interface");
            public static IStateEvent NavigatedToView => new StateEvent("NavigatedToView", "Navigated To View", "Navigated To View");
            public static IStateEvent Intitalized  => new StateEvent("Intitalized", "View Intitalized", "View Intitalized");
            public static IStateEvent CurrentEntityChanged => new StateEvent("CurrentEntityChanged", "View Current Entity Changed", "View Current Entity Changed");
        }
    }


}
