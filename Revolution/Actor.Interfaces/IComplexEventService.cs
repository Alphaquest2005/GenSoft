using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IComplexEventService: IService<IComplexEventService>
    {
        string ActorId { get; }
        IComplexEventAction ComplexEventAction { get; }
        ISystemProcess Process { get; }
    }
}
