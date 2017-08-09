using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IEntityUpdated))]
    public class EntityUpdated : ProcessSystemMessage, IEntityUpdated
    {
        public EntityUpdated() { }
        public IDynamicEntity Entity { get; }
        
        public EntityUpdated(IDynamicEntity entity,IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
           

        }

        public string EntityType => Entity.EntityType;

    }
}
