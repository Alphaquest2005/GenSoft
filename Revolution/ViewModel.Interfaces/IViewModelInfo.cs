using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewModelInfo
    {
        
        int ProcessId { get; }
        string Key { get; }
        IViewInfo ViewInfo { get; }
        List<IViewModelEventSubscription<IViewModel, IEvent>> Subscriptions { get; }

        List<IViewModelEventPublication<IViewModel, IEvent>> Publications { get; }

        List<IViewModelEventCommand<IViewModel, IEvent>> Commands { get; }
        Type ViewModelType { get; }
        Type Orientation { get; }
        int Priority { get; }
        List<IViewModelInfo> ViewModelInfos { get; }
        IViewAttributeDisplayProperties DisplayProperties { get; }
        ViewModelState ViewModelState { get; set; }
        Visibility Visibility { get; set; }
    }

    

    


    public interface IViewInfo
    {
        string Name { get; }
        string Symbol { get; }
        string Description { get; }
    }

    public interface IEntityViewInfo:IViewInfo
    {
        IDynamicEntityType EntityType { get; }
        EntityRelationshipOrdinality Ordinality { get; }
    }
}
