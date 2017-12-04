using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IAddOrGetEntityWithChanges))]

    public class AddOrGetEntityWithChanges : ProcessSystemMessage, IAddOrGetEntityWithChanges
    {
        public AddOrGetEntityWithChanges()
        {
           
        }
        public Dictionary<string, dynamic> Changes { get; }
        
        public AddOrGetEntityWithChanges(IDynamicEntityType entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("AddOrGetEntityWithChanges", new Dictionary<string, object>(){{ "Changes", changes }, { "EntityType", entityType } }),processInfo, process, source)
        {
            Contract.Requires(changes.Count > 0);
            Changes = changes;
            EntityType = entityType;
        }

        public IDynamicEntityType EntityType { get; }
    }
}