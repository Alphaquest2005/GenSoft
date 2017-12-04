using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(ICurrentEntityChanged))]
    public class CurrentEntityChanged : ProcessSystemMessage, ICurrentEntityChanged
    {
        public CurrentEntityChanged() { }
        public IDynamicEntity Entity { get; }
        public CurrentEntityChanged(IDynamicEntity entity, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CurrentEntityChanged", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity?.EntityType ?? DynamicEntity.NullEntity.EntityType } }), processInfo, process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
        }

        public IDynamicEntityType EntityType => Entity?.EntityType ?? DynamicEntity.NullEntity.EntityType;
    }
}
