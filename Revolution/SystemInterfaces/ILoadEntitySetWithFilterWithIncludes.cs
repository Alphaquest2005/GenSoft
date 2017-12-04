using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SystemInterfaces
{
    
    public interface ILoadEntitySetWithFilterWithIncludes : IProcessSystemMessage, IEntityRequest
    {
        List<Expression<Func<IDynamicEntity, bool>>> Filter { get; }
        List<Expression<Func<IDynamicEntity, object>>> Includes { get; }
    }
}
