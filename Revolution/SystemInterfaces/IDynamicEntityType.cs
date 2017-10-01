using System;
using System.Collections.Generic;
using JB.Collections.Reactive;

namespace SystemInterfaces
{
    public interface IDynamicEntityType
    {
        string Name { get; }
        string EntitySetName { get; }
        List<IEntityKeyValuePair> Properties { get; }
        
        Dictionary<string, List<dynamic>> CalculatedProperties { get; }

        ObservableDictionary<string, List<dynamic>> CachedProperties { get; }
        ObservableDictionary<string, string> CachedEntityProperties { get; }
    }
}