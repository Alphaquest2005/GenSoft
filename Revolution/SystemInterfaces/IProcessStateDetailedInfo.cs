using System;

namespace SystemInterfaces
{
    
    public interface IProcessStateInfo
    {
        ISystemProcess Process { get; }
        IState State { get; }
        

        IStateInfo ToStateInfo();
    }
}