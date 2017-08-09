using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IUpdateProcessStateList))]
    public class UpdateProcessStateList : ProcessSystemMessage, IUpdateProcessStateList
    {
        public UpdateProcessStateList() { }
        public UpdateProcessStateList(string entityType,IProcessStateList state, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            State = state;
            EntityType = entityType;
        }

        public IProcessStateList State { get; }
        public string EntityType { get; }
    }
}