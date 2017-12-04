using System;
using SystemInterfaces;

namespace Actor.Interfaces
{
    public enum ActionTrigger
    {
        Partial,
        All,
        Any
    }
    
    public interface IComplexEventAction : IComplexEvent
    {
        Type ExpectedMessageType { get; }
        IProcessStateInfo ProcessInfo { get; }
        IProcessAction Action { get; }

        ActionTrigger ActionTrigger { get;}

    }
}
