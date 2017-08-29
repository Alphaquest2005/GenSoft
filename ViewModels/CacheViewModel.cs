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
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;

using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewModels
{
    [Export(typeof(ICacheViewModel))]
    public class CacheViewModel : DynamicViewModel<EntityListViewModel>, ICacheViewModel
    {

        public CacheViewModel() { }
        public CacheViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new EntityListViewModel(viewInfo, eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            this.WireEvents();


        }


       
        public ReactiveProperty<IProcessStateList> State => this.ViewModel.State;


        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State
        {
            get { throw new NotImplementedException(); }
        }

        public ReactiveProperty<IDynamicEntity> CurrentEntity => this.ViewModel.CurrentEntity;
        public IEntityKeyValuePair CurrentProperty => this.ViewModel.CurrentProperty;


        public ObservableDictionary<string, dynamic> ChangeTracking => this.ViewModel.ChangeTracking;
        public ObservableList<IDynamicEntity> ParentEntities => this.ViewModel.ParentEntities;

        public ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet => this.ViewModel.EntitySet;



        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities => this.ViewModel.SelectedEntities;
        public ObservableBindingList<IDynamicEntity> ChangeTrackingList => this.ViewModel.ChangeTrackingList;
        
    }

   
}



