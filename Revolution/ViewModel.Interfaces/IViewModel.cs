using System;
using System.Collections.Generic;
using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;


namespace ViewModel.Interfaces
{
    
    public interface IViewModel:IProcessSource
    {
        IViewInfo ViewInfo { get; }
        ISystemProcess Process { get; }
        
        List<IViewModelEventSubscription<IViewModel, IEvent>> EventSubscriptions { get; }
        List<IViewModelEventPublication<IViewModel, IEvent>> EventPublications { get; }
        Dictionary<string, ReactiveCommand<IViewModel>> Commands { get; }
        List<IViewModelEventCommand<IViewModel, IEvent>> CommandInfo { get; }

        

        Type Orientation { get; }
        Type ViewModelType { get; }
        int Priority { get; }
        ObservableList<IViewModel> ViewModels { get; }

        ReactiveProperty<dynamic> SelectedViewModel { get; }
        ReactiveProperty<dynamic> ViewModelState { get; }
        ReactiveProperty<dynamic> Visibility { get; }
    }
}
