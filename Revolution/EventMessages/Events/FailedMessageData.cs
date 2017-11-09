using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IFailedMessageData))]
    public class FailedMessageData : ProcessSystemMessage, IFailedMessageData
    {
      public FailedMessageData() { }
        public dynamic Data { get; set; }
        

        public FailedMessageData(dynamic data,IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("FailedMessageData", new Dictionary<string, object>() { { "Data", data } }), processInfo, process, source)
        {
            Data = data;
        }
    }
}