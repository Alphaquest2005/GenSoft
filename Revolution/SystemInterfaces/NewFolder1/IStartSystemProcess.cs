using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IStartSystemProcess : IMessage
    {
        int ProcessToBeStartedId { get; }
    }
}
