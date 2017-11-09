using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntityWithChangesFound))]
    public class EntityWithChangesFound : ProcessSystemMessage, IEntityWithChangesFound 
    {
        public EntityWithChangesFound() { }
        public EntityWithChangesFound(IDynamicEntity entity, Dictionary<string, dynamic> changes, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("EntityWithChangesFound", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType }, { "Changes", changes } }), processInfo, process, source)
        {
            Entity = entity;
            Changes = changes;
        }

        public IDynamicEntity Entity { get; set; }
        public Dictionary<string, dynamic> Changes { get; }

        public IDynamicEntityType EntityType => Entity.EntityType;
    }
   
}