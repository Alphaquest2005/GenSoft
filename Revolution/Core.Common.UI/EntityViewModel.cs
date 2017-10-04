using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using FluentValidation;
using FluentValidation.Results;
using GenSoft.Interfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionData.Context;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;


namespace Core.Common.UI
{

    public class EntityViewModel : BaseViewModel<EntityViewModel>, IEntityViewModel
    {
        public EntityViewModel(IEntityViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation,
            int priority, IViewAttributeDisplayProperties displayProperties) : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation,
            priority)
        {
            ViewInfo = viewInfo;
            DisplayProperties = displayProperties;
            State.Value = new ProcessStateEntity(process, viewInfo.EntityType.DefaultEntity(), new StateInfo(process.Id, "IntialState", "Inital","first op"));
        }

        public IEntityKeyValuePair CurrentProperty { get; } = new EntityKeyValuePair(null, null,new ViewAttributeDisplayProperties(new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()), new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())));
        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();

        private ReactiveProperty<IProcessStateEntity> _state = new ReactiveProperty<IProcessStateEntity>(null, ReactivePropertyMode.DistinctUntilChanged);
        public new IEntityViewInfo ViewInfo { get; }

        public ReactiveProperty<IProcessStateEntity> State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value); }
        }

        
        public void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        public ObservableList<IDynamicEntity> ParentEntities { get; } = new ObservableList<IDynamicEntity>();
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        
        private ReactiveProperty<IDynamicEntity> _currentEntity = new ReactiveProperty<IDynamicEntity>(null, ReactivePropertyMode.DistinctUntilChanged);
        public ReactiveProperty<IDynamicEntity> CurrentEntity
        {
            get { return _currentEntity; }
            set { this.RaiseAndSetIfChanged(ref _currentEntity, value); }
        }

    }

    public class ObservableViewModel : BaseViewModel<ObservableViewModel>
    {
        public ObservableViewModel(IViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation,
            int priority) : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation,
            priority)
        {
            
        }

       
        public void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

    }
}