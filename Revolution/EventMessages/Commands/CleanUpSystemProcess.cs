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
        public ISystemProcess ProcessToBeCleanedUp { get; }

        public CleanUpSystemProcess(ISystemProcess processToBeCleanedUp, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CleanUpSystemProcess", new Dictionary<string, object>() { { "ProcessToBeCleanedUpId", processToBeCleanedUp } }), processInfo, process, source)
        {
            ProcessToBeCleanedUp = processToBeCleanedUp;
        }
    }
}