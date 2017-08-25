using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{
    
    public interface IUpdateEntityWithChanges :  IEntityRequest
    {
        Dictionary<string, object> Changes { get; }
        IDynamicEntityCore Entity { get; }
    }

    public interface IAddOrGetEntityWithChanges : IEntityRequest
    {
        Dictionary<string, object> Changes { get; }
    }
}