using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using SystemInterfaces;
using Core.Common.UI;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;
using Type = System.Type;

namespace ViewModels
{
    [Export(typeof(IEntityViewModel))]
    public class EntityDetailsViewModel : DynamicViewModel<EntityViewModel>, IEntityViewModel
    {

        public EntityDetailsViewModel(){}

        public EntityDetailsViewModel(ISystemProcess domainProcess, IEntityViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority,
            IViewAttributeDisplayProperties displayProperties) : base(new EntityViewModel(viewInfo, eventSubscriptions,
            eventPublications, commandInfo, domainProcess, orientation, priority, displayProperties))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            ViewInfo = viewInfo;

            State = this.ViewModel.State;
            CurrentEntity = this.ViewModel.CurrentEntity;
            ChangeTracking = this.ViewModel.ChangeTracking;
            DisplayProperties = this.ViewModel.DisplayProperties;
            ParentEntities = this.ViewModel.ParentEntities;
            this.WireEvents();
            this.State.Subscribe(x => UpdateCurrentEntity(x));
            this.ViewModel.CurrentEntity.Subscribe(UpdateStateEntity);
            CurrentProperty.Subscribe(x => OnValueChanged(x));

        }

        private void UpdateStateEntity(IDynamicEntity dynamicEntity)
        {
            if (!Equals(State.Value.Entity, dynamicEntity))
                State.Value.Entity = dynamicEntity;
        }

        private void UpdateCurrentEntity(IProcessStateEntity processStateEntity)
        {
            if(!Equals(ViewModel.CurrentEntity.Value, processStateEntity?.Entity))
            if(processStateEntity?.Entity == null) Debugger.Break();
            this.ViewModel.CurrentEntity.Value = processStateEntity?.Entity;
        }


        public new IEntityViewInfo ViewInfo { get; }
        public ReactiveProperty<IProcessStateEntity> State { get; } 

        //potential problem state different from current entity!
        public ReactiveProperty<IDynamicEntity> CurrentEntity { get; }

        public string SuggestedName => ParentEntities.Select(x => x.Properties["Name"].ToString()).Aggregate((c, n) => $"{c}-{n}") ?? CurrentEntity.Value.Properties["Name"].ToString();

        public ReactiveProperty<IEntityKeyValuePair> CurrentProperty { get; } = new ReactiveProperty<IEntityKeyValuePair>();



        private void OnValueChanged(object entityKeyValuePair)
        {
            
            if (entityKeyValuePair == null) return;
            if(RowState.Value == SystemInterfaces.RowState.Modified)
            ChangeTracking.AddOrUpdate(CurrentProperty.Value.Key, CurrentProperty.Value.Value);
        }


        public ObservableDictionary<string, dynamic> ChangeTracking { get; } 
        public ObservableList<IDynamicEntity> ParentEntities { get; }
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        
    }

   

}



