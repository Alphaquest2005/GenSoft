using System;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface ICreateEntityService : IProcessSystemMessage
    {
        Type ActorType { get; }
        object Action { get; }
    }
}
