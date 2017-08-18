using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SystemInterfaces;
using Common.DataEntites;
using FluentValidation;
using FluentValidation.Results;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionData.Context;
using RevolutionEntities.Process;
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
            State.Value = new ProcessStateEntity(process, new DynamicEntity("Null", -1), new StateInfo(process.Id, "IntialState", "Inital","first op"));
        }

        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();

        private ReactiveProperty<IProcessStateEntity> _state = new ReactiveProperty<IProcessStateEntity>(null, ReactivePropertyMode.DistinctUntilChanged);
        public ReactiveProperty<IProcessStateEntity> State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

    }

}