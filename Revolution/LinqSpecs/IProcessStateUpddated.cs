using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IProcessStateUpddated : IMessage
    {
        Type EntityType { get; }
        IProcessStateMessage<IEntityId> StateMessage { get; }
    }
}
