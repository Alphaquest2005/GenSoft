﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(EntitySetLoaded))]
    public class EntitySetLoaded : ProcessSystemMessage, IEntitySetLoaded
    {
        public EntitySetLoaded() { }
        public IList<IDynamicEntity> EntitySet { get; }
        

        public EntitySetLoaded(IDynamicEntityType entityType, IList<IDynamicEntity> entities, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("EntitySetLoaded", new Dictionary<string, object>() { { "EntitySet", entities }, { "EntityType", entityType } }), processInfo, process, source)
        {
            EntitySet = entities;
            EntityType = entityType;
        }
        public IDynamicEntityType EntityType { get; }
    }
}
