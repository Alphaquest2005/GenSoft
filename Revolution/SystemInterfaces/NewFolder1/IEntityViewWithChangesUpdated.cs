using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IEntityViewWithChangesUpdated<TView> : IMessage
    {
        TView Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }
}