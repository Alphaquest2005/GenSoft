using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(ICacheUpdated))]
    public class CacheUpdated : ProcessSystemMessage, ICacheUpdated
    {
        public CacheUpdated() { }
        public IDynamicEntityType EntityType { get; }

        public CacheUpdated(IDynamicEntityType entityType, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("CacheUpdated", new Dictionary<string, object>() { { "EntityType", entityType } }), processInfo, process, source)
        {
            Contract.Requires(entityType != null);
            EntityType = entityType;

        }

    }
}