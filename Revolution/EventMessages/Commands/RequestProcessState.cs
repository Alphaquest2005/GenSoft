using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IRequestProcessState))]


    public class RequestProcessState : ProcessSystemMessage, IRequestProcessState
    {
        public RequestProcessState() { }
        public RequestProcessState(IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("RequestProcessState", new Dictionary<string, object>()), processInfo,process, source)
        {
        }

    }
}