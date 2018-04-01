using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IDeleteEntity))]
    public class DeleteEntity : ProcessSystemMessage, IDeleteEntity
    {
        public DeleteEntity() { }
        public IDynamicEntity Entity { get; }

        public DeleteEntity(IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("CreateEntity", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType } }), processInfo, process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
        }
        public IDynamicEntityType EntityType => Entity.EntityType;
    }
   
}
