using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IEntityViewLoaded<out TView> : IProcessSystemMessage where TView : IEntityId
    {
        IEnumerable<TView> Entities { get; }
    }
}
