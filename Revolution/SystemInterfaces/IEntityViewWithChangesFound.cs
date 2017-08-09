using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntityWithChangesFound : IEntityRequest
    {
        IDynamicEntity Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }

    
    public interface IEntityWithChangesUpdated : IEntityRequest
    {
        IDynamicEntity Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }
}
