using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Common;
using ViewModel.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelInfo : IViewModelInfo
    {
        public ViewModelInfo(int processId, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> subscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> publications, List<IViewModelEventCommand<IViewModel, IEvent>> commands, Type viewModelType, Type orientation, int priority, IViewAttributeDisplayProperties displayProperties)
        {
            ProcessId = processId;
            Subscriptions = subscriptions;
            ViewModelType = viewModelType;
            Orientation = orientation;
            Priority = priority;
            DisplayProperties = displayProperties;
            ViewInfo = viewInfo;
            Commands = commands;
            Publications = publications;
        }

        public int ProcessId { get; }
        public string Key => ViewInfo.Name;
        public IViewInfo ViewInfo { get; }
        public List<IViewModelEventSubscription<IViewModel, IEvent>> Subscriptions { get; }
        public List<IViewModelEventPublication<IViewModel, IEvent>> Publications { get; }
        public List<IViewModelEventCommand<IViewModel, IEvent>> Commands { get; }
        public Type ViewModelType { get; }
        public Type Orientation { get; }
        public int Priority { get; }
        public List<IViewModelInfo> ViewModelInfos { get; } = new List<IViewModelInfo>();
        public IViewAttributeDisplayProperties DisplayProperties { get; }
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
        public EntityViewInfo(string name, string symbol, string description, IDynamicEntityType entityType) : base(name,symbol,description)
        {
            EntityType = entityType;
            
        }
        public IDynamicEntityType EntityType { get; }
        
    }

   

}