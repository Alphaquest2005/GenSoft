using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(ICleanUpSystemProcess))]
    public class CleanUpSystemProcess : ProcessSystemMessage, ICleanUpSystemProcess
    {
        public CleanUpSystemProcess() { }
        public int ProcessToBeCleanedUpId { get; }

        public CleanUpSystemProcess(int processId, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CleanUpSystemProcess", new Dictionary<string, object>() { { "ProcessToBeCleanedUpId", processId }}), processInfo, process, source)
        {
            ProcessToBeCleanedUpId = processId;
        }
    }
}