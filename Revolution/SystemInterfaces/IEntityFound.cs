using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    public interface IEntityFound : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
