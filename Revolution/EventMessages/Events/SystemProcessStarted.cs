using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IProcessLogCreated))]
    public class SystemProcessStarted : ProcessSystemMessage, ISystemProcessStarted
    {
        public SystemProcessStarted(IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("SystemProcessStarted", new Dictionary<string, object>()), processInfo, process, source)
        {
        }
    }
}