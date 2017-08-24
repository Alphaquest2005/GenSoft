using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{

    [Export(typeof(ICreateEntity))]
    public class CreateEntity : ProcessSystemMessage, ICreateEntity
    {
        public CreateEntity() { }
        public IDynamicEntity Entity { get; }
        
        public CreateEntity(IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
        }
        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}
