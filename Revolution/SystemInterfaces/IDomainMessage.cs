using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    public interface IDomainMessage : IProcessSystemMessage
    {
        string Type { get; }
        IDynamicEntity Entity { get; }
    }
}
