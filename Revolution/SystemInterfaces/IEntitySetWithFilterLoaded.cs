using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithFilterLoaded : IEntityRequest
    {
        IList<IDynamicEntity> Entities { get; }
    }
}
