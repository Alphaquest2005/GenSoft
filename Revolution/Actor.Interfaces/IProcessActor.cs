using System;
using System.Collections.Concurrent;
using SystemInterfaces;
using Akka.Actor;

namespace Actor.Interfaces
{
    
    public interface IProcessService:IAgent, IService<IProcessService>
    {
        ISystemProcess Process { get; }
        //ConcurrentDictionary<Type, IProcessStateMessage> ProcessStateMessages { get; }
        IActorRef ActorRef { get; }
        
    }

    public interface IEntityDataServiceActor<TService> : IAgent
    {
    }
    

    public interface IEntityDataServiceManager
    {
    }
}
