using System.Collections.Generic;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    public class LoadEntitySetWithChanges : ProcessSystemMessage, ILoadEntitySetWithChanges
    {
        public LoadEntitySetWithChanges(){}
        public LoadEntitySetWithChanges(string match,IDynamicEntityType entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
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