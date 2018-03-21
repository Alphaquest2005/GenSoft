using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace EventMessages.Commands
{
    [Export(typeof(IStartAddin))]
    public class StartAddin : ProcessSystemMessage, IStartAddin
    {
        public StartAddin(){}
        
        public StartAddin( IAddinAction action, IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("StartSystemProcess", new Dictionary<string, object>()), processInfo, process, source)
        {
            Action = action;
            Entity = entity;
        }

        public IAddinAction Action { get; }
        public IDynamicEntity Entity { get; }
    }
}
