using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IUpdateCache))]
    public class UpdateCache : ProcessSystemMessage, IUpdateCache
    {
        public UpdateCache() { }
        public IDynamicEntity Entity { get; }

        public UpdateCache(IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("CacheUpdated", new Dictionary<string, object>() { { "Entity", entity } }), processInfo, process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;

        }

        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}