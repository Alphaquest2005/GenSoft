using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface ILoadEntitySet :  IEntityRequest
    {
       
    }
    public interface IGetEntitySetWithChanges : IEntityRequest
    {
        string MatchType { get; }
        Dictionary<string, object> Changes { get; }
    }
}
