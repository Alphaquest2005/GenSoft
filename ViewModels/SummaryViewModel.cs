﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using Core.Common.UI;
using FluentValidation;

using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;

using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewModels
{
    [Export(typeof(ISummaryListViewModel))]
    public class SummaryListViewModel : DynamicViewModel<EntityListViewModel>, ISummaryListViewModel
    {

        public SummaryListViewModel() { }
        public SummaryListViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new EntityListViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();
            

        }


        public ReactiveProperty<IProcessStateList> State => this.ViewModel.State;

        private IEntityKeyValuePair _currentProperty;

        public IEntityKeyValuePair CurrentProperty
        {
            get { return _currentProperty; }
            set
            {
                _currentProperty = value;
                _currentProperty?.WhenAnyValue(x => x.Value).Subscribe(x => OnValueChanged(x));
            }
        }



        private void OnValueChanged(object entityKeyValuePair)
        {
            if (RowState.Value == SystemInterfaces.RowState.Modified)
                ChangeTracking.AddOrUpdate(_currentProperty.Key, _currentProperty.Value);
        }


        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State => new ReactiveProperty<IProcessStateEntity>(new ProcessStateEntity(State.Value.Process, CurrentEntity.Value, State.Value.StateInfo.ToStateInfo()));

        public ReactiveProperty<IDynamicEntity> CurrentEntity => this.ViewModel.CurrentEntity;
        

        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;

        public ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet => this.ViewModel.EntitySet;



        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities => this.ViewModel.SelectedEntities;
        public ObservableBindingList<IDynamicEntity> ChangeTrackingList => this.ViewModel.ChangeTrackingList;
        
    }

   
}



