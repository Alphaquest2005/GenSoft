using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IGetEntityById))]
    public class GetEntityById : ProcessSystemMessage, IGetEntityById
    {
        public GetEntityById() { }
        public int EntityId { get; }
        public IDynamicEntityType EntityType { get; }

        public GetEntityById( int entityId, IDynamicEntityType entityType , IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("GetEntityById", new Dictionary<string, object>() { { "EntityId", entityId }, { "EntityType", entityType } }), processInfo,process, source)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}
