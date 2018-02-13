using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IProcessEventFailure))]
    public class ProcessEventFailure: ProcessSystemMessage, IProcessEventFailure
    {
        public ProcessEventFailure() { }
        public Type FailedEventType { get; set; }
        public IProcessSystemMessage FailedEventMessage { get; set; }
        public Type ExpectedEventType { get; set; }
        public Exception Exception { get; set; }
        

        public ProcessEventFailure(Type failedEventType, IProcessSystemMessage failedEventMessage, Type expectedEventType, Exception exception, IStateEventInfo processInfo, ISystemSource source)
            :base(new DynamicObject("ProcessEventFailure", new Dictionary<string, object>() { { "FailedEventType", failedEventType }, { "ExpectedEventType", expectedEventType }, { "Exception", exception } }), processInfo,failedEventMessage.Process,source)
        {
            FailedEventType = failedEventType;
            //TODO: need to implement serialization
            FailedEventMessage = failedEventMessage;
            ExpectedEventType = expectedEventType;
            Exception = exception;
            
        }
    }
}
