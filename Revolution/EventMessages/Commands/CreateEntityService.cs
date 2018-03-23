using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(ICreateEntityService))]
    public class CreateEntityService : ProcessSystemMessage, ICreateEntityService
    {
        public CreateEntityService() { }
        public Type ActorType { get; }
        public object Action { get; }
        public string ActorId { get; }
        public IEntityRequest InitialMessage { get;}

        public CreateEntityService(string actorId,Type actorType, object action, IEntityRequest initialMessage, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CreateEntityService", new Dictionary<string, object>() { { "ActorType", actorType }, { "Action", action }, { "ProcessInfo", processInfo }, {"InitialMessage", initialMessage} }), processInfo, process, source)
        {
            ActorType = actorType;
            Action = action;
            ProcessInfo = processInfo;
            ActorId = ActorId;
            InitialMessage = initialMessage;
        }

        public new IStateCommandInfo ProcessInfo { get; }
    }
}