using System;

namespace SystemInterfaces
{
    
    public interface IProcessEventFailure:IProcessSystemMessage
    {
        Type FailedEventType { get; set; }
        IProcessSystemMessage FailedEventMessage { get; set; }
        Type ExpectedEventType { get; set; }
        Exception Exception { get; set; }
    }
}
