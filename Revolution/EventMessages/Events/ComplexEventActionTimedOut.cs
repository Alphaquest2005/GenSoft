using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IComplexEventActionTimedOut))]
    public class ComplexEventActionTimedOut : ProcessSystemMessage, IComplexEventActionTimedOut
    {
        public ComplexEventActionTimedOut() { }
        public IComplexEventAction Action { get; }
        
        public ComplexEventActionTimedOut(IComplexEventAction action, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("ComplexEventActionTimedOut", new Dictionary<string, object>(){{ "Action", action } }), processInfo,process, source)
        {
            Contract.Requires(action != null);
            Action = action;
           
        }
    }
}
