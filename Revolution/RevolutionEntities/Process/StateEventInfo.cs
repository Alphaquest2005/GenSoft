using System;
using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateEventInfo : ProcessStateInfo, IStateEventInfo
    {
        public new IStateEvent State { get; }
        
        public StateEventInfo(ISystemProcess process, string name, string status, string notes, string subject, string data, IStateCommand expectedCommand, Guid eventKey = default(Guid)) : base(process, new StateEvent(name,status, notes,subject,data,expectedCommand), eventKey)
        {
            State = new StateEvent(name, status, notes, subject, data, expectedCommand);
        }

        public StateEventInfo(ISystemProcess process, IStateEvent state, Guid eventKey = default(Guid)) :base(process, state, eventKey)
        {
            State = state;
        }
    }
}