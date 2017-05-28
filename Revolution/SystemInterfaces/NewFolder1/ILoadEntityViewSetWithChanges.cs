using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface ILoadEntityViewSetWithChanges<out TEntityView,out TMatchType> :IMessage, IEntityViewRequest<TEntityView> where TEntityView : IEntityView where TMatchType:IMatchType
    {
        Dictionary<string, object> Changes { get; }
    }
}