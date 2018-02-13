﻿using System;
using System.Collections.Immutable;
using System.Diagnostics;
using SystemInterfaces;
using Actor.Interfaces;

using Common;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities.Process;
using RevolutionLogger;
using Utilities;

namespace DataServices.Actors
{
    public class BaseActor<T>: IAgent, IProcessSource
    {
        public ISystemSource Source { get; }
        public ImmutableList<IProcessSystemMessage> OutMessages = ImmutableList<IProcessSystemMessage>.Empty;
        public ISystemProcess Process { get; }
        public BaseActor(string actorId,ISystemProcess process)
        {
            Process = process;
            ActorId = actorId;
            Source = new Source(Guid.NewGuid(), $"PersistentActor:{typeof(T).GetFriendlyName()}-{ActorId}" , new SourceType(typeof(BaseActor<T>)),process, process.MachineInfo);
            
        }

        public string ActorId { get; }

        internal void PublishProcesError(IProcessSystemMessage msg, Exception ex, Type expectedMessageType)
        {
            var outMsg = new ProcessEventFailure(failedEventType: msg.GetType(),
                failedEventMessage: msg,
                expectedEventType: expectedMessageType,
                exception: ex,
                source: Source, processInfo: new StateEventInfo(msg.Process, RevolutionData.Context.Process.Events.Error));
            Logger.Log(LoggingLevel.Error, $"Error:ProcessId:{msg.ProcessInfo.Process}, ProcessStatus:{msg.ProcessInfo.State.Status}, ExceptionMessage: {ex.Message}|||| {ex.StackTrace}");
            EventMessageBus.Current.Publish(outMsg);
            EventMessageBus.Current.Publish(new RequestProcessLog(new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CreateLog), msg.Process,Source));
            OutMessages = OutMessages.Add(outMsg);
        }

        internal void Publish(dynamic msg)
        {
           
            EventMessageBus.Current.Publish(msg);
            OutMessages = OutMessages.Add(msg);
        }
        //public override string PersistenceId
        //{
        //    get
        //    {

        //        var path = Context.Self.Path.ToStringWithUid();
        //        var res =  path.Substring(path.LastIndexOf("#") + 1);
        //        return "Actor-" + typeof (T).GetFriendlyName() + "-" + res;
        //    }
        //}
        //protected override void OnPersistRejected(Exception cause, object @event, long sequenceNr)
        //{
        //    base.OnPersistRejected(cause, @event, sequenceNr);
        //    Debugger.Break();
        //}

        //protected override void OnPersistFailure(Exception cause, object @event, long sequenceNr)
        //{
        //    base.OnPersistFailure(cause, @event, sequenceNr);
        //    Debugger.Break();
        //}

        public string UserId => this.Source.SourceName;
        
    }


}