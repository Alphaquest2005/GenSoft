using System;
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

        public EntityDetailsViewModel(){}
        
        public EntityDetailsViewModel(ISystemProcess process, IEntityViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority, IViewAttributeDisplayProperties displayProperties) : base(new EntityViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority, displayProperties))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();
            this.State.WhenAnyValue(x => x.Value, x => x.Value.Entity).Subscribe(UpdateCurrentEntity);
            this.ViewModel.CurrentEntity.WhenAnyValue(x => x.Value).Subscribe(UpdateStateEntity);

        }

        private void UpdateStateEntity(IDynamicEntity dynamicEntity)
        {
            if (!Equals(State.Value.Entity, dynamicEntity))
                State.Value.Entity = dynamicEntity;
        }

        private void UpdateCurrentEntity(Tuple<IProcessStateEntity,IDynamicEntity> processStateEntity)
        {
            if(!Equals(ViewModel.CurrentEntity.Value, processStateEntity.Item1.Entity))
            this.ViewModel.CurrentEntity.Value = processStateEntity.Item1.Entity;
        }


        public ReactiveProperty<IProcessStateEntity> State => this.ViewModel.State;

        //potential problem state different from current entity!
        public ReactiveProperty<IDynamicEntity> CurrentEntity => this.ViewModel.CurrentEntity;

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
            if(RowState.Value == SystemInterfaces.RowState.Modified)
            ChangeTracking.AddOrUpdate(_currentProperty.Key, _currentProperty.Value);
        }


        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
        public ObservableList<IDynamicEntity> ParentEntities => this.ViewModel.ParentEntities;
        public IViewAttributeDisplayProperties DisplayProperties => this.ViewModel.DisplayProperties;
    }

   

}



