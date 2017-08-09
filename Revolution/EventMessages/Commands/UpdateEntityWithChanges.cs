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
        public IDynamicEntity Entity { get; }

        public UpdateEntityWithChanges(IDynamicEntity entity, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(changes.Count > 0);
            Entity = entity;
            Changes = changes;
            
        }


        public string EntityType => Entity.EntityType;
    }
}