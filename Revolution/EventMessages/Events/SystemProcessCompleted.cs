using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(ISystemProcessCompleted))]
    public class SystemProcessCompleted : ProcessSystemMessage, ISystemProcessCompleted
    {
        public SystemProcessCompleted() { }
        public SystemProcessCompleted(IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("SystemProcessCompleted", new Dictionary<string, object>()), processInfo, process, source)
        {
        }
    }
}