using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IUpdateProcessStateList))]
    public class UpdateProcessStateList : ProcessSystemMessage, IUpdateProcessStateList
    {
        public UpdateProcessStateList() { }
        public UpdateProcessStateList(IDynamicEntityType entityType,IProcessStateList state, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("UpdateProcessStateList", new Dictionary<string, object>() { { "State", state }, { "EntityType", entityType}}), processInfo, process, source)
        {
            State = state;
            EntityType = entityType;
        }

        public IProcessStateList State { get; }
        public IDynamicEntityType EntityType { get; }
    }
}