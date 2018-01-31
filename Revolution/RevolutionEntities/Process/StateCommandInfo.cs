using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateCommandInfo : ProcessStateInfo, IStateCommandInfo
    {
        public StateCommandInfo(ISystemProcess process, IStateCommand state):base(process, state)
        {
            State = state;
           
        }


        public new IStateCommand State { get; }
    }
}