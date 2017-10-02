using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface ICurrentEntityChanged:IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }

    public interface IMainEntityChanged : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
