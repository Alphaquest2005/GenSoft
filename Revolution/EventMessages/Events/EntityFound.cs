using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IEntityFound))]
    public class EntityFound : ProcessSystemMessage, IEntityFound
    {
        public EntityFound() { }
        public EntityFound(IDynamicEntity entity, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entity = entity;
        }

        public IDynamicEntity Entity { get; }

        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}