using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IEntitySetWithFilterLoaded))]
    public class EntitySetWithFilterLoaded : ProcessSystemMessage, IEntitySetWithFilterLoaded
    {
        //Todo: missing filter property
        public EntitySetWithFilterLoaded() { }
        public IList<IDynamicEntity> Entities { get; }
        

        public EntitySetWithFilterLoaded(IDynamicEntityType entityType, IList<IDynamicEntity> entities, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("EntitySetWithFilterLoaded", new Dictionary<string, object>() { { "EntitySet", entities }, { "EntityType", entityType } }), processInfo, process, source)
        {
            Entities = entities;
            EntityType = entityType;
        }

        public IDynamicEntityType EntityType { get; }
    }
}
