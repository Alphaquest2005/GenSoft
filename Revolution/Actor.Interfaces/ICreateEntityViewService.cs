using System;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface ICreateEntityViewService : IProcessSystemMessage
    {
        Type ActorType { get; }
        object Action { get; }
    }
}
