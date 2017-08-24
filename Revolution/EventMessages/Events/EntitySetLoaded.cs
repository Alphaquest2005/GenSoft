using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(EntitySetLoaded))]
    public class EntitySetLoaded : ProcessSystemMessage, IEntitySetLoaded
    {
        public EntitySetLoaded() { }
        public IList<IDynamicEntity> Entities { get; }
        

        public EntitySetLoaded(IDynamicEntityType entityType, IList<IDynamicEntity> entities, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entities = entities;
            EntityType = entityType;
        }
        public IDynamicEntityType EntityType { get; }
    }
}
