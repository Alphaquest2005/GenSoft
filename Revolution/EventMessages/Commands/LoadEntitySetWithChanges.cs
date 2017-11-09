using System.Collections.Generic;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    public class GetEntitySetWithChanges : ProcessSystemMessage, IGetEntitySetWithChanges
    {
        public GetEntitySetWithChanges(){}
        public GetEntitySetWithChanges(string match,IDynamicEntityType entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("GetEntitySetWithChanges", new Dictionary<string, object>() { { "EntityType", entityType }, { "Changes", changes }, { "MatchType", match } }), processInfo, process, source)
        {
            EntityType = entityType;
            Changes = changes;
            MatchType = match;
        }
        public IDynamicEntityType EntityType { get; }
        public Dictionary<string, dynamic> Changes { get; }

        public string MatchType { get; }
    }
}