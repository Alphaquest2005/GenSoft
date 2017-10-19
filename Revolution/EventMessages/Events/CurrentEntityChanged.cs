using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{
    [Export(typeof(ICurrentEntityChanged))]
    public class CurrentEntityChanged : ProcessSystemMessage, ICurrentEntityChanged
    {
        public CurrentEntityChanged() { }
        public IDynamicEntity Entity { get; }
        public CurrentEntityChanged(IDynamicEntity entity, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            Entity = entity;
        }

        public IDynamicEntityType EntityType => Entity?.EntityType;
    }

    [Export(typeof(IDomainMessage))]
    public class DomainMessage : ProcessSystemMessage, IDomainMessage
    {
        public DomainMessage() { }
        public string Type { get; }
        public IDynamicEntity Entity { get; }
        public DomainMessage(string type,IDynamicEntity entity, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            Entity = entity;
            Type = type;
        }

        public IDynamicEntityType EntityType => Entity?.EntityType;
    }
}
