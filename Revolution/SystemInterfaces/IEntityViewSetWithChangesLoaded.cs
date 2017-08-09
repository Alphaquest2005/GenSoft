using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithChangesLoaded : IEntityRequest
    {
        List<IDynamicEntity> EntitySet { get; }
        Dictionary<string, object> Changes { get; }
    }

}
