using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;

namespace ViewModel.Interfaces
{
    
    public interface IViewModel:IProcessSource
    {
        IViewInfo ViewInfo { get; }
        ISystemProcess Process { get; }
        
        List<IViewModelEventSubscription<IViewModel, IEvent>> EventSubscriptions { get; }
        List<IViewModelEventPublication<IViewModel, IEvent>> EventPublications { get; }
        Dictionary<string, ReactiveCommand<IViewModel, Unit>> Commands { get; }
        List<IViewModelEventCommand<IViewModel, IEvent>> CommandInfo { get; }

        

        Type Orientation { get; }
        Type ViewModelType { get; }
        int Priority { get; }
        ObservableList<IViewModel> ViewModels { get; }

        
    }
}
