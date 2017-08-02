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
    [Export(typeof(IEntityDetailsViewModel<>))]
    public class EntityDetailsViewModel<TView> : DynamicViewModel<ObservableViewModel<TView>>, IEntityDetailsViewModel<TView> where TView : IEntityView
    {

        public EntityDetailsViewModel() { }
        public EntityDetailsViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new ObservableViewModel<TView>(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();


        }


        public ReactiveProperty<IProcessState<TView>> State => this.ViewModel.State;
        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
    }

    [Export(typeof(IEntityDetailsViewModel<IPatientDetailsInfo>))]
    public class PatientDetailsViewModel : EntityDetailsViewModel<IPatientDetailsInfo>
    {
        public PatientDetailsViewModel()
        {
        }

        public PatientDetailsViewModel(ISystemProcess process, IViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) 
            : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation, priority)
        {
        }

    }

    [Export(typeof(IEntityDetailsViewModel<IPatientVitalsInfo>))]
    public class PatientVitalsViewModel : EntityDetailsViewModel<IPatientVitalsInfo>
    {
        public PatientVitalsViewModel()
        {
        }

        public PatientVitalsViewModel(ISystemProcess process, IViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority)
            : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation, priority)
        {
        }

    }


}



