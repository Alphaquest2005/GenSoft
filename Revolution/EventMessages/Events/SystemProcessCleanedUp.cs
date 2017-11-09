using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(ISystemProcessCleanedUp))]
    public class SystemProcessCleanedUp : ProcessSystemMessage, ISystemProcessCleanedUp
    {
        public SystemProcessCleanedUp() { }
        public SystemProcessCleanedUp(IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("SystemProcessCleanedUp", new Dictionary<string, object>()), processInfo, process, source)
        {
        }
    }
}