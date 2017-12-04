using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IEntitySetLoaded: IEntityRequest
    {
        IList<IDynamicEntity> EntitySet { get; }
    }
}
