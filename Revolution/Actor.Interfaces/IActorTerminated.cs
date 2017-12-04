using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IActorTerminated : IProcessSystemMessage
    {
        IComplexEventService Actor { get; }
    }
}
