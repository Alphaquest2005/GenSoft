using System;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(ILoadEntitySetWithChanges))]


    public class LoadEntitySet : ProcessSystemMessage, ILoadEntitySet
    { 
        public LoadEntitySet() { }
        public LoadEntitySet(IDynamicEntityType entityType, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            EntityType = entityType;
        }
        public IDynamicEntityType EntityType { get; }
    }
}
