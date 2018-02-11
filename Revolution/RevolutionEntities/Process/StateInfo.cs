using System;
using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateInfo : ProcessStateInfo, IStateInfo
    {
        public StateInfo(ISystemProcess process, IState state, Guid eventKey = default(Guid)) : base(process, state, eventKey)
        {
        }

        public StateInfo(ISystemProcess process, string name, string status, string notes, string subject, string data, Guid eventKey = default(Guid)) : base(process, new State(name,status, notes, subject, data), eventKey)
        {
           
        }

        

    }
}