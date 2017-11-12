using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IDomainMessage))]
    public class DomainMessage : ProcessSystemMessage, IDomainMessage
    {
        public DomainMessage() { }
        public string Type { get; }
        public IDynamicEntity Entity { get; }
        public DomainMessage(string type,IDynamicEntity entity, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("DomainMessage", new Dictionary<string, object>() { { "Type", type }, { "Entity", entity }, { "EntityType", entity.EntityType } }), processInfo, process, source)
        {
            Entity = entity;
            Type = type;
        }

        public IDynamicEntityType EntityType => Entity?.EntityType;
    }
}