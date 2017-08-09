using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface ILoadEntitySetWithFilterWithIncludes : IProcessSystemMessage, IEntityRequest
    {
        List<Expression<Func<IDynamicEntity, bool>>> Filter { get; }
        List<Expression<Func<IDynamicEntity, object>>> Includes { get; }
    }
}
