using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(ISystemStarted))]
    public class SystemStarted : ProcessSystemMessage, ISystemStarted
    {
        public SystemStarted() { }
        public SystemStarted(IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("SystemStarted", new Dictionary<string, object>()), processInfo, process, source)
        {
        }
    }
}
