using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithChangesLoaded : IEntityRequest
    {
        List<IDynamicEntity> EntitySet { get; }
        Dictionary<string, object> Changes { get; }
    }

}
