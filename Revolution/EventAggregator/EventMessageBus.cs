using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using SystemInterfaces;
using Common;
using GenSoft.Entities;
using Process.WorkFlow;
using RevolutionLogger;
using Utilities;

namespace EventAggregator
{
    public class EventMessageBus//: Reactive.EventAggregator.EventAggregator
    {
        static Reactive.EventAggregator.EventAggregator ea = new Reactive.EventAggregator.EventAggregator();
        static ConcurrentDictionary<string, dynamic> eventStore = new ConcurrentDictionary<string, dynamic>();
        public static ISystemSource Source => new Source(Guid.NewGuid(), $"EventMessageBus", new RevolutionEntities.Process.SourceType(typeof(EventMessageBus)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
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
            Task.Run(() =>
            {
                var key = $"{typeof(TEvent).GetFriendlyName()}-{caller.Process.Id}";
                eventStore.TryGetValue(key, out dynamic sampleEvent);
                if(sampleEvent != null)
                    Publish(sampleEvent, Source);
            });
            return ea.GetEvent<TEvent>();
        }


        public void Publish<TEvent>(TEvent sampleEvent, ISource sender) where TEvent : IProcessSystemMessage
        {
            try
            {
                Contract.Requires(sender != null || sampleEvent != null);

                Logger.Log(LoggingLevel.Info,
                    $"Sender:{sender.SourceName} | PublishEvent : {typeof(TEvent).GetFriendlyName()}| ProcessInfo:Status-{sampleEvent?.ProcessInfo?.State?.Status}|| ProcessId-{sampleEvent.Process.Id} || EntityType-{(sampleEvent is IEntityRequest er?er.EntityType.Name:"")}");
                
                Task.Run(() =>
                {
                    var key = $"{typeof(TEvent).GetFriendlyName()}-{sampleEvent.Process.Id}";
                    eventStore.AddOrUpdate(key, sampleEvent);
                });
                Task.Run(() => { ea.Publish(sampleEvent);});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
