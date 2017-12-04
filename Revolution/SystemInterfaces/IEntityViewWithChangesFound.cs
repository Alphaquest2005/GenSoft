using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IEntityWithChangesFound : IEntityRequest
    {
        IDynamicEntity Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }

    
    public interface IEntityWithChangesUpdated : IEntityRequest
    {
        IDynamicEntityCore Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }
}
