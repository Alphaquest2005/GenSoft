using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IEntitySetWithFilterLoaded))]
    public class EntitySetWithFilterLoaded : ProcessSystemMessage, IEntitySetWithFilterLoaded
    {
        public EntitySetWithFilterLoaded() { }
        public IList<IDynamicEntity> Entities { get; }
        

        public EntitySetWithFilterLoaded(string entityType, IList<IDynamicEntity> entities, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entities = entities;
            EntityType = entityType;
        }

        public string EntityType { get; }
    }
}
