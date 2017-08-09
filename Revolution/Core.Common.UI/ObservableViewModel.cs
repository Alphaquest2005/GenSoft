using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SystemInterfaces;
using FluentValidation;
using FluentValidation.Results;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionData.Context;
using ViewModel.Interfaces;
using ViewModelInterfaces;


namespace Core.Common.UI
{

    public class ObservableViewModel : BaseViewModel<ObservableViewModel>
    {
        public ObservableViewModel(IViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation,
            int priority) : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation,
            priority)
        {
        }

        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();

        public void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

    }

}