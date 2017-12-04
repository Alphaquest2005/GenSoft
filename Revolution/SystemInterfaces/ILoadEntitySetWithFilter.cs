using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SystemInterfaces
{
    
    public interface ILoadEntitySetWithFilter : IProcessSystemMessage, IEntityRequest
    {
        List<Expression<Func<IDynamicEntity, bool>>> Filter { get; }
    }
}
