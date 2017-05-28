using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{
    
    public interface IUpdateEntityWithChanges<out TEntity> : IMessage, IEntityRequest<TEntity> where TEntity : IEntity
    {
        Dictionary<string, object> Changes { get; }
        int EntityId { get; }
    }
}