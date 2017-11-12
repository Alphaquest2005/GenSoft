using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IGetEntitySetWithChanges))]


    public class LoadEntitySet : ProcessSystemMessage, ILoadEntitySet
    { 
        public LoadEntitySet() { }
        public LoadEntitySet(IDynamicEntityType entityType, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("LoadEntitySet", new Dictionary<string, object>() { { "EntityType", entityType }}), processInfo,process, source)
        {
            EntityType = entityType;
        }
        public IDynamicEntityType EntityType { get; }
    }
}
