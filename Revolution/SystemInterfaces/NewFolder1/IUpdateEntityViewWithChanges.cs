using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IUpdateEntityViewWithChanges<out TEntityView> : IMessage, IEntityViewRequest<TEntityView> where TEntityView : IEntityView
    {
        Dictionary<string, object> Changes { get; }
        int EntityId { get; }
    }
}