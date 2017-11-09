using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IProcessStateMessage))]
    public class UpdateProcessStateEntity : ProcessSystemMessage, IProcessStateMessage
    { 
        public UpdateProcessStateEntity() { }
        public UpdateProcessStateEntity(IProcessStateEntity state, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("UpdateProcessStateEntity", new Dictionary<string, object>() { { "State", state }, { "EntityType", state.Entity.EntityType } }), processInfo, process, source)
        {
            State = state;
        }

        public IProcessStateEntity State { get; }
        public IDynamicEntityType EntityType => State.Entity.EntityType;
    }
}