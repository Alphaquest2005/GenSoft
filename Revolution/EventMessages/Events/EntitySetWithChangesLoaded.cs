﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntitySetWithChangesLoaded))]
    public class EntitySetWithChangesLoaded:ProcessSystemMessage, IEntitySetWithChangesLoaded
    {
        public EntitySetWithChangesLoaded() { }
        public List<IDynamicEntity> EntitySet { get; }
        public Dictionary<string, object> Changes { get; }

        public EntitySetWithChangesLoaded(IDynamicEntityType entityType,List<IDynamicEntity> entitySet, Dictionary<string, object> changes, IStateEventInfo stateEventInfo, ISystemProcess process, ISystemSource source)
            :base(new DynamicObject("EntitySetWithChangesLoaded", new Dictionary<string, object>() { { "EntitySet", entitySet }, { "EntityType", entityType }, { "Changes", changes } }), stateEventInfo, process, source)
        {
            EntitySet = entitySet;
            Changes = changes;
            EntityType = entityType;
        }

        public IDynamicEntityType EntityType { get; }
    }
}
