using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemInterfaces;
using Common;
using GenSoft.Entities;
using Process.WorkFlow;
using RevolutionLogger;
using Utilities;
using Type = System.Type;

namespace EventAggregator
{
    public class EventMessageBus//: Reactive.EventAggregator.EventAggregator
    {
        static Reactive.EventAggregator.EventAggregator ea = new Reactive.EventAggregator.EventAggregator();
        static ConcurrentDictionary<dynamic, dynamic> eventStore = new ConcurrentDictionary<dynamic, dynamic>();
        public static ISystemSource Source => new Source(Guid.NewGuid(), $"EventMessageBus", new RevolutionEntities.Process.SourceType(typeof(EventMessageBus)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        static EventMessageBus()
        {
            Current = new EventMessageBus();
        }

        public static EventMessageBus Current { get; }

        public IObservable<TEvent> GetEvent<TEvent>(IProcessStateInfo processInfo, ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null && processInfo != null);
            if(processInfo.EventKey == Guid.Empty) Debugger.Break();
            var ge = ea.GetEvent<TEvent>();
            Task.Run(() =>
            {
                var er = typeof(TEvent) as IEntityRequest;
                Logger.Log(LoggingLevel.Info,
                    $"Caller:{caller.SourceName} | GetEvent : {typeof(TEvent).GetFriendlyName()}|ProcessInfo:Status-{processInfo.State.Status}| ProcessId-{caller.Process?.Id} || EntityType-{(er != null ? er.EntityType.Name : "")}");
            });
            Task.Run(() =>
            {
                var key = $"{typeof(TEvent).GetFriendlyName()}-{caller.Process.Id}";
                
                eventStore.TryGetValue("Pub-" + key, out dynamic actualEvent);
                if (actualEvent != null)
                {
                    Publish(actualEvent, Source);
                    eventStore.TryRemove("Pub-" + key, out actualEvent);
                }
                eventStore.AddOrUpdate("Get-" + key, null);
                return ea.GetEvent<TEvent>();
            });
            return ge;
        }
        


        public void Publish<TEvent>(TEvent sampleEvent, ISource sender) where TEvent : IProcessSystemMessage
        {
            try
            {
                Contract.Requires(sender != null || sampleEvent != null);
                if (sampleEvent.ProcessInfo.EventKey != Guid.Empty) Debugger.Break();
                Task.Run(() =>
                {
                    Logger.Log(LoggingLevel.Info,
                        $"Sender:{sender.SourceName} | PublishEvent : {typeof(TEvent).GetFriendlyName()}| ProcessInfo:Status-{sampleEvent?.ProcessInfo?.State?.Status}|| ProcessId-{sampleEvent.Process.Id} || EntityType-{(sampleEvent is IEntityRequest er ? er.EntityType.Name : "")}");
                });
                Task.Run(() =>
                {

                    var key = $"I{typeof(TEvent).GetFriendlyName()}-{sampleEvent.Process.Id}";
                    if(!eventStore.ContainsKey("Get-"+key)) eventStore.AddOrUpdate("Pub-"+key, sampleEvent);
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
