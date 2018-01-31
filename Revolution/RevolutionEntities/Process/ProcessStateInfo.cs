using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public abstract class ProcessStateInfo: IProcessStateInfo
    {
        protected ProcessStateInfo(ISystemProcess process, IState state)
        {
            Process = process;
            State = state;
        }

        public ISystemProcess Process { get; }
        public IState State { get; }
        public IStateInfo ToStateInfo() => new StateInfo(Process, State);
    }


}