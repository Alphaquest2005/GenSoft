using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(ILoadEntitySetWithFilter))]


    public class LoadEntitySetWithFilter : ProcessSystemMessage, ILoadEntitySetWithFilter
    {
        public LoadEntitySetWithFilter() { }
        public List<Expression<Func<IDynamicEntity, bool>>> Filter { get; }
      
      
        public LoadEntitySetWithFilter(string entityType,List<Expression<Func<IDynamicEntity,bool>>> filter, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(filter != null);
            Filter = filter;
            EntityType = entityType;
        }
        public string EntityType { get; }
    }
}
