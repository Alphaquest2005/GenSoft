using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntityWithChangesFound))]
    public class EntityWithChangesFound : ProcessSystemMessage, IEntityWithChangesFound 
    {
        public EntityWithChangesFound() { }
        public EntityWithChangesFound(IDynamicEntity entity, Dictionary<string, dynamic> changes, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entity = entity;
            Changes = changes;
        }

        public IDynamicEntity Entity { get; set; }
        public Dictionary<string, dynamic> Changes { get; }

        public IDynamicEntityType EntityType => Entity.EntityType;
    }
   
}