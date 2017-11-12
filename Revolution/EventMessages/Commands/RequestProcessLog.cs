using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IRequestProcessLog))]

    public class RequestProcessLog:ProcessSystemMessage, IRequestProcessLog
    {
        public RequestProcessLog() { }
        public RequestProcessLog(IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            :base(new DynamicObject("RequestProcessLog", new Dictionary<string, object>()), processInfo,process, source)
        {
            
        }
    }
}
