using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IProcessStateMessage))]
    public class UpdateProcessStateEntity : ProcessSystemMessage, IProcessStateMessage
    { 
        public UpdateProcessStateEntity() { }
        public UpdateProcessStateEntity(IProcessStateEntity state, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            State = state;
        }

        public IProcessStateEntity State { get; }
        public IDynamicEntityType EntityType => State.Entity.EntityType;
    }
}