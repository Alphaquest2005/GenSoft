using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{

    [Export(typeof(ICreateEntity))]
    public class CreateEntity : ProcessSystemMessage, ICreateEntity
    {
        public CreateEntity() { }
        public IDynamicEntity Entity { get; }
        
        public CreateEntity(IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("CreateEntity", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType } }),processInfo,process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
        }
        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}
