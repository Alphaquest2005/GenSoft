using System;
using System.Collections.Generic;
using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewModelEventSubscription<in TViewModel, in TEvent> : IEventSubscription<TEvent> where TViewModel : IViewModel where TEvent : IEvent
    {
        Action<TViewModel, TEvent> Action { get; }
        IEnumerable<Func<TViewModel, TEvent, bool>> ActionPredicate { get; }
        Type ViewModelType { get; }

        string Key { get; }
    }
}
