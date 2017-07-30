using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using Core.Common.UI;
using EF.Entities;
using FluentValidation;
using Interfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionEntities.Process;
using Utilities;
using ValidationSets;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewModels
{
    [Export(typeof(ISummaryListViewModel<>))]
    public class SummaryListViewModel<TView> : DynamicViewModel<ObservableListViewModel<TView>>, ISummaryListViewModel<TView> where TView : IEntityView
    {

        public SummaryListViewModel() { }
        public SummaryListViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new ObservableListViewModel<TView>(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();


        }


        IEntityListViewModel<TView> IEntityListViewModel<TView>.Instance => (IEntityListViewModel<TView>) Instance;
        public ReactiveProperty<IProcessStateList<TView>> State => this.ViewModel.State;


        ReactiveProperty<IProcessState<TView>> IEntityViewModel<TView>.State => new ReactiveProperty<IProcessState<TView>>(this.ViewModel.State.Value, ReactivePropertyMode.DistinctUntilChanged);
        public ReactiveProperty<TView> CurrentEntity => this.ViewModel.CurrentEntity;

        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;

        public ReactiveProperty<ObservableList<TView>> EntitySet => this.ViewModel.EntitySet;



        public ReactiveProperty<ObservableList<TView>> SelectedEntities => this.ViewModel.SelectedEntities;
        public ObservableBindingList<TView> ChangeTrackingList => this.ViewModel.ChangeTrackingList;
        
    }

    [Export(typeof(ISummaryListViewModel<IPatientVisitInfo>))]
    public class PatientVisitViewModel : SummaryListViewModel<IPatientVisitInfo>
    {
        public PatientVisitViewModel()
        {
        }

        public PatientVisitViewModel(ISystemProcess process, IViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) 
            : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation, priority)
        {
        }

    }

}



