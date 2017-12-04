using System.Collections.Generic;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface ICreateProcessActor : IProcessSystemMessage
    {
        string ActorName { get; }
        IList<IComplexEventAction> ComplexEvents { get; }
    }
}
