using System;
using SystemInterfaces;
using Common;
using EventAggregator;
using EventMessages.Events;
using Process.WorkFlow;
using RevolutionEntities.Process;
using RevolutionLogger;

namespace EFRepository
{
    public class BaseRepository:IProcessSource
    {

        public static ISystemSource Source => new Source(Guid.NewGuid(), $"EntityRepository:<>", new SourceType(typeof(BaseRepository)),Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        internal static void PublishProcesError(IProcessSystemMessage msg, Exception ex, Type expectedMessageType)
        {
            var outMsg = new ProcessEventFailure(failedEventType: msg.GetType(),
                failedEventMessage: msg,
                expectedEventType: expectedMessageType,
                exception: ex,
                source: Source, processInfo: new StateEventInfo(msg.Process, RevolutionData.Context.Process.Events.Error));
            Logger.Log(LoggingLevel.Error, $"Error:ProcessId:{msg.ProcessInfo.Process}, ProcessStatus:{msg.ProcessInfo.State.Status}, ExceptionMessage: {ex.Message}|||| {ex.StackTrace}");
            EventMessageBus.Current.Publish(outMsg, Source);
        }

        ISystemSource IProcessSource.Source => BaseRepository.Source;
    }
}