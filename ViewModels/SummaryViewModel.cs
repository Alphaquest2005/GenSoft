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
    [Export(typeof(ISummaryListViewModel))]
    public class SummaryListViewModel : DynamicViewModel<ObservableListViewModel>, ISummaryListViewModel
    {

        public SummaryListViewModel() { }
        public SummaryListViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new ObservableListViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();


        }


        IEntityListViewModel IEntityListViewModel.Instance => (IEntityListViewModel) Instance;
        public ReactiveProperty<IProcessStateList> State => this.ViewModel.State;


        ReactiveProperty<IProcessState> IEntityViewModel.State => new ReactiveProperty<IProcessState>(this.ViewModel.State.Value, ReactivePropertyMode.DistinctUntilChanged);
        public ReactiveProperty<IDynamicEntity> CurrentEntity => this.ViewModel.CurrentEntity;

        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;

        public ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet => this.ViewModel.EntitySet;



        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities => this.ViewModel.SelectedEntities;
        public ObservableBindingList<IDynamicEntity> ChangeTrackingList => this.ViewModel.ChangeTrackingList;
        
    }

   
}



