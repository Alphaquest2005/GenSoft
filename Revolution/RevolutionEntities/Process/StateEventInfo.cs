using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateEventInfo : ProcessStateInfo, IStateEventInfo
    {
        public new IStateEvent State { get; }
        
        public StateEventInfo(ISystemProcess process, string name, string status, string notes, IStateCommand expectedCommand) : base(process, new StateEvent(name,status, notes,expectedCommand))
        {
            State = new StateEvent(name, status, notes, expectedCommand);
        }

        public StateEventInfo(ISystemProcess process, IStateEvent state):base(process, state)
        {
            State = state;
        }
    }
}