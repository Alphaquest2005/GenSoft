using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithFilterWithIncludesLoaded : IEntityRequest
    {
        IList<IDynamicEntity> Entities { get; }
        IList<Expression<Func<IDynamicEntity, object>>> Includes { get; }
    }
}
