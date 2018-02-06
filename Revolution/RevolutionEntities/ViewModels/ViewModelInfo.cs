using System;
using System.Collections.Generic;
using System.Windows;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelInfo : IViewModelInfo
    {
        public ViewModelInfo(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> subscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> publications, List<IViewModelEventCommand<IViewModel, IEvent>> commands, Type viewModelType, Type orientation, int priority, IViewAttributeDisplayProperties displayProperties, IReadOnlyList<IViewModelInfo> viewModelInfos)
        {
            Process = process;
            Subscriptions = subscriptions;
            ViewModelType = viewModelType;
            Orientation = orientation;
            Priority = priority;
            DisplayProperties = displayProperties;
            ViewModelInfos = viewModelInfos;
            ViewInfo = viewInfo;
            Commands = commands;
            Publications = publications;
        }

        public ISystemProcess Process { get; }
        public string Key => ViewInfo.Name;
        public IViewInfo ViewInfo { get; }
        public IReadOnlyList<IViewModelEventSubscription<IViewModel, IEvent>> Subscriptions { get; }
        public IReadOnlyList<IViewModelEventPublication<IViewModel, IEvent>> Publications { get; }
        public IReadOnlyList<IViewModelEventCommand<IViewModel, IEvent>> Commands { get; }
        public Type ViewModelType { get; }
        public Type Orientation { get; }
        public int Priority { get; }
        public IReadOnlyList<IViewModelInfo> ViewModelInfos { get; }
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        public ViewModelState ViewModelState { get; set; }
        public Visibility Visibility { get; set; }
    }

    public class ViewInfo : IViewInfo
    {
        public ViewInfo(string name, string symbol, string description)
        {
            Name = name;
            Symbol = symbol;
            Description = description;
            
        }

        public string Name { get; }
        public string Symbol { get; }
        public string Description { get; }
        
    }

    public class EntityViewInfo :ViewInfo, IEntityViewInfo
    {
        public EntityViewInfo(string name, string symbol, string description, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality) : base(name,symbol,description)
        {
            EntityType = entityType;
            Ordinality = ordinality;
        }
        public IDynamicEntityType EntityType { get; }
        public EntityRelationshipOrdinality Ordinality { get; }
    }

   

}