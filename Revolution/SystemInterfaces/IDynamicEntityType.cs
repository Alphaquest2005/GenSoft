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

        ObservableDictionary<string, Dictionary<int, dynamic>> CachedProperties { get; }
        ObservableDictionary<string, string> PropertyParentEntityType { get; }
        IIntelliList<IDynamicEntityType> ChildEntities { get;  }
        IIntelliList<IDynamicEntityType> ParentEntities { get;  }
    }

    public interface IIntelliList<T>:IList<T>
    {
        T SelectedItem { get; set; }
    }
}