using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IGetEntityWithChanges))]

    public class GetEntityWithChanges : ProcessSystemMessage, IGetEntityWithChanges 
    {
        public GetEntityWithChanges() { }
        public GetEntityWithChanges(IDynamicEntityCore entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("GetEntityWithChanges", new Dictionary<string, object>() { { "EntityType", entityType }, { "Changes", changes } }), processInfo,process, source)
        {
            Changes = changes;
            EntityType = entityType.EntityType;
        }

        public Dictionary<string, dynamic> Changes { get; }

        public IDynamicEntityType EntityType { get; }
    }
}