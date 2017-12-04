using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IUpdateEntityWithChanges :  IEntityRequest
    {
        Dictionary<string, object> Changes { get; }
        IDynamicEntity Entity { get; }
    }

    public interface IAddOrGetEntityWithChanges : IEntityRequest
    {
        Dictionary<string, object> Changes { get; }
    }
}