using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IUpdateEntityWithChanges))]

    public class UpdateEntityWithChanges : ProcessSystemMessage, IUpdateEntityWithChanges
    {
        public UpdateEntityWithChanges() { }
        public Dictionary<string, dynamic> Changes { get; }
        public IDynamicEntityCore Entity { get; }

        public UpdateEntityWithChanges(IDynamicEntityCore entity, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(changes.Count > 0);
            Entity = entity;
            Changes = changes;
            
        }


        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}