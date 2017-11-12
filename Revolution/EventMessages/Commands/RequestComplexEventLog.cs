using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IRequestComplexEventLog))]

    public class RequestComplexEventLog : ProcessSystemMessage, IRequestComplexEventLog
    {
        public RequestComplexEventLog() { }
        public RequestComplexEventLog(IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("RequestComplexEventLog", new Dictionary<string, object>()), processInfo,process, source)
        {
        }
    }
}
