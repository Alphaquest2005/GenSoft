using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IAddOrGetEntityWithChanges<out TEntity> : IMessage, IEntityRequest<TEntity> where TEntity : IEntity
    {
        Dictionary<string, object> Changes { get; }
    }
}