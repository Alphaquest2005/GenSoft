using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Utilities;
using ViewModel.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewEventPublication<TViewModel, TEvent> :EventPublication<TViewModel, TEvent>, IViewModelEventPublication<IViewModel, IEvent> where TViewModel:IViewModel where TEvent:IEvent
    {
        
        public ViewEventPublication(string key, Func<TViewModel, IObservable<dynamic>> subject, IEnumerable<Func<TViewModel, bool>> subjectPredicate, Func<TViewModel, IViewEventPublicationParameter> messageData)
                                    :base(key, subject, subjectPredicate, messageData)
        {
            MessageData =  (Func<IViewModel, IViewEventPublicationParameter>)base.MessageData.Convert(typeof(IViewModel));
            SubjectPredicate = base.SubjectPredicate.Select(x => (Func<IViewModel, bool>)x.Convert(typeof(IViewModel))).ToList();
            Subject = (Func<IViewModel, IObservable<dynamic>>)base.Subject.Convert(typeof(IViewModel)); 
        }


        //public ViewEventPublication(string key, Func<TViewModel, IObservable<bool>> subject, IEnumerable<Func<TViewModel, bool>> subjectPredicate, Func<TViewModel, IViewEventPublicationParameter> messageData)
        //    : base(key, (Func<TViewModel, IObservable<dynamic>>)subject.Convert(typeof(TViewModel), typeof(IObservable<dynamic>)), subjectPredicate, messageData)
        //{
        //    MessageData = (Func<IViewModel, IViewEventPublicationParameter>)base.MessageData.Convert(typeof(IViewModel), typeof(IViewEventPublicationParameter));
        //    SubjectPredicate = base.SubjectPredicate.Select(x => (Func<IViewModel, bool>)x.Convert(typeof(IViewModel), typeof(bool))).ToList();
        //    Subject = (Func<IViewModel, IObservable<dynamic>>)base.Subject.Convert(typeof(IViewModel), typeof(IObservable<dynamic>));
        //}

        public new Func<IViewModel, IObservable<dynamic>> Subject { get; }
        public new IEnumerable<Func<IViewModel, bool>> SubjectPredicate { get; }
        public new Func<IViewModel, IViewEventPublicationParameter> MessageData { get; }
       
    }
}