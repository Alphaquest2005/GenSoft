using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Documents;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace EventMessages.Commands
{
    [Export(typeof(IStartSystemProcess))]

    public class StartSystemProcess:ProcessSystemMessage, IStartSystemProcess
    {
        public StartSystemProcess() { }
        public int ProcessToBeStartedId { get; }

        public StartSystemProcess(int processId,IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source):base(new DynamicObject("StartSystemProcess", new Dictionary<string, object>()), processInfo, process, source)
        {
            ProcessToBeStartedId = processId;
        }
    }
}
