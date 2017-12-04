using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IComplexEventActionTimedOut:IProcessSystemMessage
    {
        IComplexEventAction Action { get; }
    }
}
