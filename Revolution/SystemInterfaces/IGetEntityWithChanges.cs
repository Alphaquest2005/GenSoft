using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IGetEntityWithChanges : IEntityRequest
    {
        Dictionary<string, object> Changes { get; }
    }
}
