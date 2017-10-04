using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(IMainEntityChanged))]
    public class MainEntityChanged : ProcessSystemMessage, IMainEntityChanged
    {
        public MainEntityChanged() { }
        public IDynamicEntity Entity { get; }
        public MainEntityChanged(IDynamicEntity entity, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            Entity = entity;
        }

        public IDynamicEntityType EntityType => Entity?.EntityType;
    }
}