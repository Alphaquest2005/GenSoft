using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntityFound))]
    public class EntityFound : ProcessSystemMessage, IEntityFound
    {
        public EntityFound() { }
        public EntityFound(IDynamicEntity entity, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("EntityFound", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType } }), processInfo, process, source)
        {
            Entity = entity;
        }

        public IDynamicEntity Entity { get; }

        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}