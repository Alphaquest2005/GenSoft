using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface ICreateComplexEventService : IProcessSystemMessage
    {
        IComplexEventService ComplexEventService { get;  }
    }
}
