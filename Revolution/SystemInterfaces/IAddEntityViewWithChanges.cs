using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IAddEntityViewWithChanges<out TEntityView> : IMessage, IEntityViewRequest<TEntityView> where TEntityView : IEntityView
    {
        Dictionary<string, object> Changes { get; }
    }
}