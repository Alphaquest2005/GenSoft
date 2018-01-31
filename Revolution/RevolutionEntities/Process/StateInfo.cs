using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public class StateInfo : ProcessStateInfo, IStateInfo
    {
        public StateInfo(ISystemProcess process, IState state) : base(process, state)
        {
        }

        public StateInfo(ISystemProcess process, string name, string status, string notes) : base(process, new State(name,status, notes))
        {
           
        }

        

    }
}