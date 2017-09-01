using System;
using System.Collections.Generic;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelEventSubscription<TViewModel, TEvent> :IViewModelEventSubscription<TViewModel, TEvent> where TViewModel : IViewModel where TEvent : IEvent
    {
        protected ViewModelEventSubscription(string key, int processId, Func<TEvent, bool> eventPredicate, IEnumerable<Func<TViewModel,TEvent, bool>> actionPredicate, Action<TViewModel, TEvent> action)
        {
            ProcessId = processId;
            EventPredicate = eventPredicate;
            ActionPredicate = actionPredicate;
            Action = action;
            Key = key;
        }

        public Type EventType { get; } = typeof (TEvent);
        public Action<TViewModel, TEvent> Action { get; }
        public Func<TEvent, bool> EventPredicate { get; }
        public IEnumerable<Func<TViewModel, TEvent, bool>> ActionPredicate { get; }
        public Type ViewModelType { get; } = typeof (TViewModel);
        public string Key { get; }
        public int ProcessId { get; }
    }
}