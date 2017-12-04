using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Core.Common.UI;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace ViewModels
{
    [Export(typeof(ICacheViewModel))]
    public class CacheViewModel : DynamicViewModel<EntityListViewModel>, ICacheViewModel
    {
        private static ICacheViewModel _instance = null;
        public new ICacheViewModel Instance => _instance;
        public CacheViewModel() { }
        public CacheViewModel(ISystemProcess process, IEntityViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority, IViewAttributeDisplayProperties displayProperties) : base(new EntityListViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority, displayProperties))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();
            if (_instance == null) _instance = this;
            

        }

        public new IEntityViewInfo ViewInfo => this.ViewModel.ViewInfo;
        public ReactiveProperty<IProcessStateList> State => this.ViewModel.State;


        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State
        {
            get { throw new NotImplementedException(); }
        }

        public ReactiveProperty<IDynamicEntity> CurrentEntity => this.ViewModel.CurrentEntity;
        public ReactiveProperty<IEntityKeyValuePair> CurrentProperty => this.ViewModel.CurrentProperty;


        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
        public ObservableList<IDynamicEntity> ParentEntities => this.ViewModel.ParentEntities;
        public IViewAttributeDisplayProperties DisplayProperties => this.ViewModel.DisplayProperties;
        

        public ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet => this.ViewModel.EntitySet;



        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities => this.ViewModel.SelectedEntities;
        public ObservableBindingList<IDynamicEntity> ChangeTrackingList => this.ViewModel.ChangeTrackingList;

        
    }

   
}



