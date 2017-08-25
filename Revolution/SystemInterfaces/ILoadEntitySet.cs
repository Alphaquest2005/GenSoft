using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

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
