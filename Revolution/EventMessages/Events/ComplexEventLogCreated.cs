using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IComplexEventLogCreated))]
    public class ComplexEventLogCreated : ProcessSystemMessage, IComplexEventLogCreated
    {
        public ComplexEventLogCreated() { }
        public IEnumerable<IComplexEventLog> EventLog { get; }

        public ComplexEventLogCreated(IEnumerable<IComplexEventLog> logs, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("ComplexEventLogCreated", new Dictionary<string, object>(){{ "EventLog", logs } }), processInfo,process, source)
        {
            EventLog = logs;
        }

    }
}
