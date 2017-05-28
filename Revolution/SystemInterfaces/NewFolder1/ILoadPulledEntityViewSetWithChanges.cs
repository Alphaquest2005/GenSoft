using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface ILoadPulledEntityViewSetWithChanges<out TEntityView, out TMatchType> : IMessage, IEntityViewRequest<TEntityView> where TEntityView : IEntityView where TMatchType : IMatchType
    {
        Dictionary<string, dynamic> Changes { get; }
        string EntityName { get; }

    }
}