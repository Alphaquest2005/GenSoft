using System;
using System.Collections.Generic;
using System.Windows;
using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewModelInfo
    {

        ISystemProcess Process { get; }
        string Key { get; }
        IViewInfo ViewInfo { get; }
        IReadOnlyList<IViewModelEventSubscription<IViewModel, IEvent>> Subscriptions { get; }

        IReadOnlyList<IViewModelEventPublication<IViewModel, IEvent>> Publications { get; }

        IReadOnlyList<IViewModelEventCommand<IViewModel, IEvent>> Commands { get; }
        Type ViewModelType { get; }
        Type Orientation { get; }
        int Priority { get; }
        IReadOnlyList<IViewModelInfo> ViewModelInfos { get; }
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
