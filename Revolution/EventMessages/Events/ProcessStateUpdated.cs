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
        public string EntityType { get;}
        public IProcessStateMessage StateMessage { get; }

        public ProcessStateUpdated(string entityType, IProcessStateMessage stateMessage, IStateEventInfo stateEventInfo, ISystemProcess process, ISystemSource source):base(stateEventInfo, process, source)
        {
            this.EntityType = entityType;
            StateMessage = stateMessage;
            
        }
    }
}
