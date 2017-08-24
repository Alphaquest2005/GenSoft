using System;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IProcessStateUpddated))]
    public class ProcessStateUpdated : ProcessSystemMessage, IProcessStateUpddated
    {
        public ProcessStateUpdated() { }
        public IDynamicEntityType EntityType { get;}
        public IProcessStateMessage StateMessage { get; }

        public ProcessStateUpdated(IDynamicEntityType entityType, IProcessStateMessage stateMessage, IStateEventInfo stateEventInfo, ISystemProcess process, ISystemSource source):base(stateEventInfo, process, source)
        {
            this.EntityType = entityType;
            StateMessage = stateMessage;
            
        }
    }
}
