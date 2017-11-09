using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IEntityUpdated))]
    public class EntityUpdated : ProcessSystemMessage, IEntityUpdated
    {
        public EntityUpdated() { }
        public IDynamicEntity Entity { get; }
        
        public EntityUpdated(IDynamicEntity entity,IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("EntityUpdated", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType } }), processInfo, process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
           

        }

        public IDynamicEntityType EntityType => Entity.EntityType;

    }
}
