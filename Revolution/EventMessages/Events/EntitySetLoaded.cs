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
        public IList<IDynamicEntity> EntitySet { get; }
        

        public EntitySetLoaded(IDynamicEntityType entityType, IList<IDynamicEntity> entities, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            EntitySet = entities;
            EntityType = entityType;
        }
        public IDynamicEntityType EntityType { get; }
    }
}
