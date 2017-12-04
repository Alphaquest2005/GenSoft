using System;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using System.Diagnostics.Contracts;

namespace RevolutionEntities.Process
{

    public class ProcessAction : IProcessAction
    {
        public ProcessAction() { }
        public Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>> Action { get; set; }
        public Func<IDynamicComplexEventParameters, IProcessStateInfo> ProcessInfo { get; set; }
        public ISourceType ExpectedSourceType { get; set; }

        public ProcessAction(Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>> action, Func<IDynamicComplexEventParameters, IStateCommandInfo> processInfo, ISourceType expectedSourceType)
        {
            Contract.Requires(action != null);
            Action = action;
            ProcessInfo = processInfo;
            ExpectedSourceType = expectedSourceType;
        }
    }
}
