using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{


    [Export(typeof(IEntityNotFound))]
    public class EntityNotFound : ProcessSystemMessage, IEntityNotFound
    {
        public EntityNotFound() { }
        public EntityNotFound(IDynamicEntity entity, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entity = entity;
        }

        public IDynamicEntity Entity { get; }

        public string EntityType => Entity.EntityType;
    }
}
