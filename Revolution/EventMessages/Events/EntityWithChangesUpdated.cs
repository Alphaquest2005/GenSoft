using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;
using System.Diagnostics.Contracts;

namespace EventMessages.Events
{
    [Export(typeof(IEntityWithChangesUpdated))]
    public class EntityWithChangesUpdated: ProcessSystemMessage, IEntityWithChangesUpdated
    {
        public EntityWithChangesUpdated() { }
        public EntityWithChangesUpdated(IDynamicEntityCore entity, Dictionary<string, dynamic> changes, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
            Changes = changes;
        }

        public IDynamicEntityCore Entity { get; set; }
        public Dictionary<string, dynamic> Changes { get; }
        public IDynamicEntityType EntityType => Entity.EntityType;

    }
}