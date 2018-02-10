using System;
using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateCommandInfo : ProcessStateInfo, IStateCommandInfo
    {
        public StateCommandInfo(ISystemProcess process, IStateCommand state, Guid eventKey = default(Guid)) :base(process, state, eventKey)
        {
            State = state;
           
        }


        public new IStateCommand State { get; }
    }
}