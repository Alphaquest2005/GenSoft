using System;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IGetEntityById))]
    public class GetEntityById : ProcessSystemMessage, IGetEntityById
    {
        public GetEntityById() { }
        public int EntityId { get; }
        public string EntityType { get; }

        public GetEntityById( int entityId, string entityType , IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}
