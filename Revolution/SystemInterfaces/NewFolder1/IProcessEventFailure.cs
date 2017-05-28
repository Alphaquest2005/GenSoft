using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IProcessEventFailure:IMessage
    {
        Type FailedEventType { get; set; }
        IMessage FailedEventMessage { get; set; }
        Type ExpectedEventType { get; set; }
        Exception Exception { get; set; }
    }
}
