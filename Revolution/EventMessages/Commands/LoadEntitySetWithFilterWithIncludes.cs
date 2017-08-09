using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(ILoadEntitySetWithFilterWithIncludes))]


    public class LoadEntitySetWithFilterWithIncludes : ProcessSystemMessage, ILoadEntitySetWithFilterWithIncludes
    {
        public LoadEntitySetWithFilterWithIncludes() { }
        public List<Expression<Func<IDynamicEntity, bool>>> Filter { get; }
        public List<Expression<Func<IDynamicEntity,dynamic>>> Includes { get; }
        
        public LoadEntitySetWithFilterWithIncludes(string entityType,List<Expression<Func<IDynamicEntity,bool>>> filter, List<Expression<Func<IDynamicEntity, dynamic>>> includes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            Contract.Requires(filter != null);
            Contract.Requires(includes != null);
            Filter = filter;
            Includes = includes;
            EntityType = entityType;

        }
        public string EntityType { get; }
    }
}
