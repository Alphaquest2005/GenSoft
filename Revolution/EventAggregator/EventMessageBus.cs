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
        static ConcurrentDictionary<dynamic, dynamic> _getEventStore = new ConcurrentDictionary<dynamic, dynamic>();
        static ConcurrentDictionary<dynamic, dynamic> _publishEventStore = new ConcurrentDictionary<dynamic, dynamic>();
        public static ISystemSource Source => new Source(Guid.NewGuid(), $"EventMessageBus", new RevolutionEntities.Process.SourceType(typeof(EventMessageBus)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        static EventMessageBus()
        {
            Current = new EventMessageBus();
        }

        public static EventMessageBus Current { get; }

        public IObservable<TEvent> GetEvent<TEvent>(IProcessStateInfo processInfo, ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null && processInfo != null);
            
             var ge = ea.GetEvent<TEvent>();

            //Task.Run(() =>
            //{
                var er = typeof(TEvent) as IEntityRequest;
                Logger.Log(LoggingLevel.Info,
                    $"Caller:{caller.SourceName} | GetEvent : {typeof(TEvent).GetFriendlyName()}|ProcessInfo:Status-{processInfo.State.Status}|ProcessInfo:SubjectData{processInfo.State.Subject}-{processInfo.State.Data}| ProcessId-{caller.Process?.Id} |Source:{caller.SourceName}-{caller.SourceId}|Key: {processInfo.EventKey}");
            //}).ConfigureAwait(false);
            
            var key = $"{typeof(TEvent).GetFriendlyName()}-{processInfo.State.Subject}-{processInfo.State.Data}-{caller.Process.Id}";

             //Task.Run(() =>
             //{
                 if (processInfo.EventKey == Guid.Empty) Debugger.Break();
                 _publishEventStore.TryGetValue("Pub-" + key, out dynamic actualEvent);

                 if (actualEvent != null)
                 {
                     //ToDo:need to change processinfo key
                     //var type = BootStrapper.BootStrapper.Container.GetConcreteType(typeof(TEvent));
                     // var newEvent =(TEvent) typeof(JsonUtilities).GetMethod("CloneJson").MakeGenericMethod(type).Invoke(null, new object[] { actualEvent });
                     // var newEvent = JsonUtilities.CloneJson<TEvent>(actualEvent);
                     

                     actualEvent.ProcessInfo.EventKey = processInfo.EventKey;
                     Task.Delay(1000).ContinueWith((t) => { EventMessageBus.Current.Publish(actualEvent, Source); }).ConfigureAwait(false);  
                    
                    
                 }

             //}).ConfigureAwait(false);

            //Task.Run(() =>
            //{
                _getEventStore.AddOrUpdate("Get-" + key, null);
            //}).ConfigureAwait(false);

            return ge;

        }
        


        public void Publish<TEvent>(TEvent sampleEvent, ISource sender) where TEvent : IProcessSystemMessage
        {
            try
            {
                Contract.Requires(sender != null || sampleEvent != null);
                
                //Task.Run(() =>
                //{
                    Logger.Log(LoggingLevel.Info,
                        $"Sender:{sender.SourceName} | PublishEvent : {typeof(TEvent).GetFriendlyName()}| ProcessInfo:Status-{sampleEvent?.ProcessInfo?.State?.Status}|ProcessInfo:SubjectData{sampleEvent?.ProcessInfo?.State.Subject}-{sampleEvent?.ProcessInfo?.State.Data}| ProcessId-{sampleEvent.Process.Id} |Source:{sender.SourceName}-{sender.SourceId}|Key:{sampleEvent.ProcessInfo.EventKey}");
                //}).ConfigureAwait(false);
                //Task.Run(() =>
                //{
                    var key = $"I{typeof(TEvent).GetFriendlyName()}-{sampleEvent?.ProcessInfo?.State.Subject}-{sampleEvent?.ProcessInfo?.State.Data}-{sampleEvent.Process.Id}";
                    _publishEventStore.AddOrUpdate("Pub-"+key, sampleEvent);
                //}).ConfigureAwait(false);
                //Task.Run(() =>
                //{
                    ea.Publish(sampleEvent);
                //}).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        
    }
}
