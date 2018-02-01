using System;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using RevolutionLogger;
using Utilities;

namespace EventAggregator
{
    public class EventMessageBus//: Reactive.EventAggregator.EventAggregator
    {
        static Reactive.EventAggregator.EventAggregator ea = new Reactive.EventAggregator.EventAggregator();

        static EventMessageBus()
        {
            Current = new EventMessageBus();
        }

        public static EventMessageBus Current { get; }

        public IObservable<TEvent> GetEvent<TEvent>(ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null);
            var er = typeof(TEvent) as IEntityRequest;
            Logger.Log( LoggingLevel.Info ,$"Caller:{caller.SourceName} | GetEvent : {typeof(TEvent).GetFriendlyName()}|| ProcessId-{caller.Process?.Id} || EntityType-{(er != null ? er.EntityType.Name : "")}");
            return ea.GetEvent<TEvent>();
        }


        public void Publish<TEvent>(TEvent sampleEvent, ISource sender) where TEvent : IProcessSystemMessage
        {
            try
            {
                Contract.Requires(sender != null || sampleEvent != null);
                Logger.Log(LoggingLevel.Info,
                    $"Sender:{sender.SourceName} | PublishEvent : {typeof(TEvent).GetFriendlyName()}| ProcessInfo:Status-{sampleEvent?.ProcessInfo?.State?.Status}|| ProcessId-{sampleEvent?.Process.Id} || EntityType-{(sampleEvent is IEntityRequest er?er.EntityType.Name:"")}");
                ea.Publish(sampleEvent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
