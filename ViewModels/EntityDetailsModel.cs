﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using Core.Common.UI;
using FluentValidation;
using JB.Collections.Reactive;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewModels
{
    [Export(typeof(IEntityViewModel))]
    public class EntityDetailsViewModel : DynamicViewModel<EntityViewModel>, IEntityViewModel
    {

        public EntityDetailsViewModel() { }
        
        public EntityDetailsViewModel(ISystemProcess process, IEntityViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new EntityViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();
           
        }


        public ReactiveProperty<IProcessStateEntity> State
        {
            get { return this.ViewModel.State; }
        }

        private IEntityKeyValuePair _currentProperty;

        public IEntityKeyValuePair CurrentProperty
        {
            get { return _currentProperty; }
            set
            {
                _currentProperty = value;
                _currentProperty?.WhenAnyValue(x => x.Value).Subscribe(x => OnValueChanged(x));}
        }



        private void OnValueChanged(object entityKeyValuePair)
        {
            ChangeTracking.AddOrUpdate(_currentProperty.Key, _currentProperty.Value);
        }


        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
    }

   

}



