using System.Collections.Generic;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IComplexEvent
    {
        string Key { get; }
        IList<IProcessExpectedEvent> Events { get; }
        int ProcessId { get; }
       

    }
}