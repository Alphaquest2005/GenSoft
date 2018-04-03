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
    public class EventLogger//: Reactive.EventAggregator.EventAggregator
    {
        public static ISystemSource Source { get; } = new Source(Guid.NewGuid(), $"EventLogger", new RevolutionEntities.Process.SourceType(typeof(EventLogger)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);

        static EventLogger()
        {
            Current = new EventLogger();
            EventMessageBus.Current.GetEvent<IProcessSystemMessage>(
                new RevolutionEntities.Process.StateEventInfo(Processes.IntialSystemProcess, new StateEvent("EventLogger", "Log Events", "", "EventLogger", "EventLogger"), Guid.NewGuid()), Source)
                .Subscribe(x => Current.LogMessages(x)); ;
        }

        public static EventLogger Current { get; }

        public void GetEvent<TEvent>(IProcessStateInfo processInfo, ISource caller) where TEvent : IProcessSystemMessage
        {
            Contract.Requires(caller != null && processInfo != null);
                var er = typeof(TEvent) as IEntityRequest;
                Logger.Log(LoggingLevel.Info,
                    $"Caller:{caller.SourceName} | GetEvent : {typeof(TEvent).GetFriendlyName()}|ProcessInfo:Status-{processInfo.State.Status}|ProcessInfo:SubjectData{processInfo.State.Subject}-{processInfo.State.Data}| ProcessId-{caller.Process?.Id} |Source:{caller.SourceName}-{caller.SourceId}|Key: {processInfo.EventKey}");
        }
        


        public void LogMessages(IProcessSystemMessage msg)
        {
            try
            {
                
                    Logger.Log(LoggingLevel.Info,
                        $"Sender:{msg.Source.SourceName} | PublishEvent : {msg.GetType().GetFriendlyName()}| ProcessInfo:Status-{msg.ProcessInfo.State.Status}|ProcessInfo:SubjectData-{msg.ProcessInfo.State.Subject}-{msg.ProcessInfo.State.Data}| ProcessId-{msg.Process.Id} |Source:{msg.Source.SourceName}-{msg.Source.SourceId}|Key:{msg.ProcessInfo.EventKey}");
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        
    }
}
