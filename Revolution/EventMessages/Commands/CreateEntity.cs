using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using JB.Collections.Reactive;

namespace EventMessages.Commands
{

    [Export(typeof(ICreateEntity))]
    public class CreateEntity : ProcessSystemMessage, ICreateEntity
    {
        public CreateEntity() { }
        public IDynamicEntity Entity { get; }
        
        public CreateEntity(IDynamicEntity entity, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CreateEntity", new Dictionary<string, object>() { { "Entity", entity }, { "EntityType", entity.EntityType } }),processInfo,process, source)
        {
            Contract.Requires(entity != null);
            Entity = entity;
        }
        public IDynamicEntityType EntityType => Entity.EntityType;
    }

    [Export(typeof(IEntityRequest))]
    public class NullEntityRequest : ProcessSystemMessage, IEntityRequest
    {
        public NullEntityRequest() { }
        public IDynamicEntity Entity { get; }

        public NullEntityRequest( IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("CreateEntity", new Dictionary<string, object>() { }), processInfo, process, source)
        {
            Contract.Requires(processInfo != null);
            Entity = new DynamicEntity(new DynamicEntityType("NullEntity", "NullEntities",new List<IEntityKeyValuePair>(),new Dictionary<string, List<dynamic>>(),new ObservableDictionary<string, Dictionary<int, dynamic>>(), new ObservableDictionary<string, string>()),0,new Dictionary<string, object>());
        }
        public IDynamicEntityType EntityType => Entity.EntityType;
    }
}
