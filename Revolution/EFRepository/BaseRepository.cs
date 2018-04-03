using System;
using SystemInterfaces;
using Common;
using EventAggregator;
using EventMessages.Events;
using Process.WorkFlow;
using RevolutionEntities.Process;
using RevolutionLogger;
using Utilities;

namespace EFRepository
{
    public class BaseRepository<TRepository>:IProcessSource
    {
     
        public ISystemSource Source { get; } = new Source(Guid.NewGuid(), $"EntityRepository:<{typeof(TRepository).GetFriendlyName()}>", new SourceType(typeof(BaseRepository<>)),Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        internal void PublishProcesError(IProcessSystemMessage msg, Exception ex, Type expectedMessageType)
        {
            var outMsg = new ProcessEventFailure(failedEventType: msg.GetType(),
                failedEventMessage: msg,
                expectedEventType: expectedMessageType,
                exception: ex,
                source: Source, processInfo: new StateEventInfo(msg.Process, RevolutionData.Context.Process.Events.Error));
            Logger.Log(LoggingLevel.Error, $"Error:ProcessId:{msg.ProcessInfo.Process}, ProcessStatus:{msg.ProcessInfo.State.Status}, ExceptionMessage: {ex.Message}|||| {ex.StackTrace}");
            EventMessageBus.Current.Publish(outMsg);
        }

        ISystemSource IProcessSource.Source { get; } = null;
    }
}