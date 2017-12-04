using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using SystemInterfaces;
using Core.Common.UI;
using JB.Collections.Reactive;
using Reactive.Bindings;

using RevolutionEntities.Process;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace ViewModels
{
    [Export(typeof(ISummaryListViewModel))]
    public class SummaryListViewModel : DynamicViewModel<EntityListViewModel>, ISummaryListViewModel
    {

        public SummaryListViewModel() { }

        public SummaryListViewModel(ISystemProcess process, IEntityViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority,
            IViewAttributeDisplayProperties displayProperties) : base(new EntityListViewModel(viewInfo,
            eventSubscriptions, eventPublications, commandInfo, process, orientation, priority, displayProperties))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;


            State = this.ViewModel.State;
            CurrentEntity = this.ViewModel.CurrentEntity;
            ChangeTracking = this.ViewModel.ChangeTracking;
            ParentEntities = this.ViewModel.ParentEntities;
            DisplayProperties = this.ViewModel.DisplayProperties;
            EntitySet = this.ViewModel.EntitySet;
            SelectedEntities = this.ViewModel.SelectedEntities;
            ChangeTrackingList = this.ViewModel.ChangeTrackingList;

            this.WireEvents();

            this.State.Subscribe(x => UpdateCurrentEntity(x));
            this.ViewModel.EntitySet.Subscribe(x => UpdateStateEntity(x));
            this.CurrentEntity.Subscribe(x => Currentvaluechanged(x));
            this.CurrentProperty.Subscribe(x => OnValueChanged(x));
        }

        private void Currentvaluechanged(IDynamicEntity dynamicEntity)
        {
           
        }

        private void UpdateStateEntity(ObservableList<IDynamicEntity> dynamicEntities)
        {
            //if (State.Value.EntitySet != dynamicEntities)
            //    State.Value.EntitySet = dynamicEntities;
        }

        private void UpdateCurrentEntity(IProcessStateList processStateEntity)
        {
            var res = processStateEntity?.EntitySet.ToList() ?? new List<IDynamicEntity>();
            if (!this.ViewModel.EntitySet.Value.ToList().SequenceEqual(res))
                this.ViewModel.EntitySet.Value = new ObservableList<IDynamicEntity>(res);
            
        }


        public new IEntityViewInfo ViewInfo => this.ViewModel.ViewInfo;
        public ReactiveProperty<IProcessStateList> State { get;  }

        public ReactiveProperty<IEntityKeyValuePair> CurrentProperty { get; } = new ReactiveProperty<IEntityKeyValuePair>();
        



        private void OnValueChanged(object entityKeyValuePair)
        {
            if (RowState.Value == SystemInterfaces.RowState.Modified)
                ChangeTracking.AddOrUpdate(CurrentProperty.Value.Key, CurrentProperty.Value);
        }


        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State { get; } = new ReactiveProperty<IProcessStateEntity>();
        //new ReactiveProperty<IProcessStateEntity>(new ProcessStateEntity(State.Value.Process, CurrentEntity.Value, State.Value.StateInfo.ToStateInfo()));

        public ReactiveProperty<IDynamicEntity> CurrentEntity { get; } 


        public ObservableDictionary<string, dynamic> ChangeTracking { get; } 
        public ObservableList<IDynamicEntity> ParentEntities { get; }
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        
        public ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; }



        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; } 
        public ObservableBindingList<IDynamicEntity> ChangeTrackingList { get; }

        
    }

   
}



