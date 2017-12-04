using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithFilterLoaded : IEntityRequest
    {
        IList<IDynamicEntity> Entities { get; }
    }
}
