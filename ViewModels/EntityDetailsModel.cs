using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using Core.Common.UI;
using FluentValidation;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewModels
{
    [Export(typeof(IEntityDetailsViewModel))]
    public class EntityDetailsViewModel : DynamicViewModel<ObservableViewModel>, IEntityDetailsViewModel
    {

        public EntityDetailsViewModel() { }
        public EntityDetailsViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new ObservableViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();


        }


        
        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
    }

   

}



