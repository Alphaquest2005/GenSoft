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
using RevolutionEntities.Process;
using RevolutionLogger;
using Utilities;
using Type = System.Type;

namespace EventAggregator
{
    public class EventStore//: Reactive.EventAggregator.EventAggregator
    {
        
        static ConcurrentDictionary<dynamic, dynamic> _getEventStore = new ConcurrentDictionary<dynamic, dynamic>();
        static ConcurrentDictionary<dynamic, dynamic> _publishEventStore = new ConcurrentDictionary<dynamic, dynamic>();
        public static ISystemSource Source { get; } = new Source(Guid.NewGuid(), $"EventStore", new RevolutionEntities.Process.SourceType(typeof(EventStore)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        static EventStore()
        {
           
            EventMessageBus.Current.GetEvent<IProcessSystemMessage>(
                new RevolutionEntities.Process.StateEventInfo(Processes.IntialSystemProcess, new StateEvent("EventStore","Save Events","","EventStore","EventStore"),Guid.NewGuid()), Source)
                .Subscribe(x => Current.SaveMessages(x));
        }
        private static readonly EventStore instance = new EventStore();
        public static EventStore Current => instance;


        public void GetEvent<TEvent>(IProcessStateInfo processInfo, ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null && processInfo != null);

            var key =
                $"{typeof(TEvent).GetFriendlyName()}-{processInfo.State.Subject}-{processInfo.State.Data}-{caller.Process.Id}";
            _getEventStore.AddOrUpdate("Get-" + key, null);
            //if (processInfo.EventKey == Guid.Empty) Debugger.Break();
            _publishEventStore.TryGetValue("Pub-" + key, out dynamic actualEvent);

            if (actualEvent != null && actualEvent.Process.Id == caller.Process.Id)
            {
                actualEvent.ProcessInfo.EventKey = processInfo.EventKey;
                actualEvent.Source = Source;
                Task.Run(() => { EventMessageBus.Current.Publish(actualEvent); }).ConfigureAwait(false);

            }

        }



        public void SaveMessages(IProcessSystemMessage msg)
        {
            try
            {
                    var key = $"I{msg.GetType().GetFriendlyName()}-{msg.ProcessInfo.State.Subject}-{msg.ProcessInfo?.State.Data}-{msg.Process.Id}";
                    _publishEventStore.AddOrUpdate("Pub-"+key, msg);
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        
    }
}
