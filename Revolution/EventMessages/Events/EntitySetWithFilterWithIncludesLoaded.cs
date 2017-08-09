using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Events
{

    [Export(typeof(IEntitySetWithFilterWithIncludesLoaded))]
    public class EntitySetWithFilterWithIncludesLoaded : ProcessSystemMessage, IEntitySetWithFilterWithIncludesLoaded
    {
        public EntitySetWithFilterWithIncludesLoaded() { }
        public IList<IDynamicEntity> Entities { get; }
        public IList<Expression<Func<IDynamicEntity, dynamic>>> Includes { get; }
        //Todo: include filter just to match name
        public EntitySetWithFilterWithIncludesLoaded(string entityType, IList<IDynamicEntity> entities, IList<Expression<Func<IDynamicEntity, dynamic>>> includes, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Entities = entities;
            Includes = includes;
            EntityType = entityType;

        }

        public string EntityType { get; }
    }
}
