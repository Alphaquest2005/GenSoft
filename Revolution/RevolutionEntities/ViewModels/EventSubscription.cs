using System;
using System.Collections.Generic;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelEventSubscription<TViewModel, TEvent> :IViewModelEventSubscription<TViewModel, TEvent> where TViewModel : IViewModel where TEvent : IEvent
    {
        protected ViewModelEventSubscription(string key, ISystemProcess process, Func<TEvent, bool> eventPredicate, IEnumerable<Func<TViewModel,TEvent, bool>> actionPredicate, Action<TViewModel, TEvent> action, IProcessStateInfo processInfo)
        {
            Process = process;
            EventPredicate = eventPredicate;
            ActionPredicate = actionPredicate;
            Action = action;
            ProcessInfo = processInfo;
            Key = key;
        }

        public Type EventType { get; } = typeof (TEvent);
        public Action<TViewModel, TEvent> Action { get; }
        public Func<TEvent, bool> EventPredicate { get; }
        public IProcessStateInfo ProcessInfo { get; }
        public IEnumerable<Func<TViewModel, TEvent, bool>> ActionPredicate { get; }
        public Type ViewModelType { get; } = typeof (TViewModel);
        public string Key { get; }
        public ISystemProcess Process { get; }
    }
}