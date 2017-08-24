using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IGetEntityWithChanges))]

    public class GetEntityWithChanges : ProcessSystemMessage, IGetEntityWithChanges 
    {
        public GetEntityWithChanges() { }
        public GetEntityWithChanges(IDynamicEntityType entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Changes = changes;
            EntityType = entityType;
        }

        public Dictionary<string, dynamic> Changes { get; }

        public IDynamicEntityType EntityType { get; }
    }
}