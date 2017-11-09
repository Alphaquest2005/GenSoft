using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IFailedMessageData))]
    public class FailedCommandData : ProcessSystemMessage, IFailedMessageData
    {
        public FailedCommandData() { }
        public dynamic Data { get; set; }


        public FailedCommandData(dynamic data, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("FailedCommandData", new Dictionary<string, object>() { { "Data", data }}), processInfo, process, source)
        {
            Data = data;
        }
    }
}