using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithFilterWithIncludesLoaded : IEntityRequest
    {
        IList<IDynamicEntity> Entities { get; }
        IList<Expression<Func<IDynamicEntity, object>>> Includes { get; }
    }
}
