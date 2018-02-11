using System;
using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public abstract class ProcessStateInfo: IProcessStateInfo
    {
        protected ProcessStateInfo(ISystemProcess process, IState state, Guid eventKey)
        {
            Process = process;
            State = state;
            EventKey = eventKey;
        }

        public ISystemProcess Process { get; }
        public IState State { get; }
        public Guid EventKey { get; set; } 
        public IStateInfo ToStateInfo() => new StateInfo(Process, State);
    }


}