using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using DomainUtilities;
using JB.Collections.Reactive;
using Reactive.Bindings;
using RevolutionEntities.Process;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;


namespace Core.Common.UI
{

    public class EntityViewModel : BaseViewModel<EntityViewModel>, IEntityViewModel
    {
        public EntityViewModel() {}
        public EntityViewModel(IEntityViewInfo viewInfo,
            List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions,
            List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications,
            List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation,
            int priority, IViewAttributeDisplayProperties displayProperties) : base(process, viewInfo, eventSubscriptions, eventPublications, commandInfo, orientation,
            priority)
        {
            ViewInfo = viewInfo;
            DisplayProperties = displayProperties;
            State.Value = new ProcessStateEntity(process, viewInfo.EntityType.DefaultEntity(), new StateInfo(process, "InitialState", "Initial","first op", "ViewModel", viewInfo.Name));
        }


        /// <summary>
        /// ///////////////////////////DO NOT DO NOT USE =>  to instancte reactive properties... Value not updating
        /// </summary>

        public ReactiveProperty<IEntityKeyValuePair> CurrentProperty { get; } = new ReactiveProperty<IEntityKeyValuePair>();// new EntityKeyValuePair(null, null,new ViewAttributeDisplayProperties(new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()), new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())));
        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();

        
        public new IEntityViewInfo ViewInfo { get; }

        public ReactiveProperty<IProcessStateEntity> State { get; } = new ReactiveProperty<IProcessStateEntity>();

        public string SuggestedName => ParentEntities.Select(x => x.Properties["Name"].ToString()).Aggregate((c, n) => $"{c}-{n}") ?? CurrentEntity.Value.Properties["Name"].ToString();
        public void NotifyPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
        }

        public ObservableList<IDynamicEntity> ParentEntities { get; } = new ObservableList<IDynamicEntity>();
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        
        
        public ReactiveProperty<IDynamicEntity> CurrentEntity { get; } = new ReactiveProperty<IDynamicEntity>();
      

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

       
        
    }
}