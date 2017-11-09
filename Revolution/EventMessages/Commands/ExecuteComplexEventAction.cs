using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IExecuteComplexEventAction))]
    public class ExecuteComplexEventAction:ProcessSystemMessage, IExecuteComplexEventAction
    {
        public ExecuteComplexEventAction() { }
        public IProcessAction Action { get;  }
        public IDynamicComplexEventParameters ComplexEventParameters { get; }


        public ExecuteComplexEventAction(IProcessAction action, IDynamicComplexEventParameters complexEventParameters, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("ExecuteComplexEventAction", new Dictionary<string, object>() { { "Action", action }, { "ComplexEventParameters", complexEventParameters } }), processInfo ,process, source)
        {
            Action = action;
            ComplexEventParameters = complexEventParameters;
        }
    }
}
