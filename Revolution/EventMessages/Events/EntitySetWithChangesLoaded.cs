using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntitySetWithChangesLoaded))]
    public class EntitySetWithChangesLoaded:ProcessSystemMessage, IEntitySetWithChangesLoaded
    {
        public EntitySetWithChangesLoaded() { }
        public List<IDynamicEntity> EntitySet { get; }
        public Dictionary<string, object> Changes { get; }

        public EntitySetWithChangesLoaded(string entityType,List<IDynamicEntity> entitySet, Dictionary<string, object> changes, IStateEventInfo stateEventInfo, ISystemProcess process, ISystemSource source):base(stateEventInfo,process, source)
        {
            EntitySet = entitySet;
            Changes = changes;
            EntityType = entityType;
        }

        public string EntityType { get; }
    }
}
