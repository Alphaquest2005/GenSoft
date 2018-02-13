using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemInterfaces;
using BootStrapper;
using Common;
using GenSoft.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Process.WorkFlow;
using RevolutionLogger;
using Utilities;
using Type = System.Type;

namespace EventAggregator
{
    public class EventMessageBus//: Reactive.EventAggregator.EventAggregator
    {
        static Reactive.EventAggregator.EventAggregator ea = new Reactive.EventAggregator.EventAggregator();
        public static ISystemSource Source => new Source(Guid.NewGuid(), $"EventMessageBus", new RevolutionEntities.Process.SourceType(typeof(EventMessageBus)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        static EventMessageBus()
        {
            Current = new EventMessageBus();
        }

        public static EventMessageBus Current { get; }

        public IObservable<TEvent> GetEvent<TEvent>(IProcessStateInfo processInfo, ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null && processInfo != null);
            Task.Run(() => EventStore.Current.GetEvent<TEvent>(processInfo, caller)).ConfigureAwait(false);
            Task.Run(() => EventLogger.Current.GetEvent<TEvent>(processInfo, caller)).ConfigureAwait(false);

            return ea.GetEvent<TEvent>();

        }
        


        public void Publish<TEvent>(TEvent sampleEvent) where TEvent : IProcessSystemMessage
        {
            try
            {
                Contract.Requires(sampleEvent != null);
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
