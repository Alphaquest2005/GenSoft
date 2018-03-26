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
        IIntelliList<IDynamicRelationshipType> ChildEntities { get;  }
        IIntelliList<IDynamicRelationshipType> ParentEntities { get;  }
        IDynamicEntityType NullEntityType();
        IDynamicEntityType ParentEntityType { get; }

        ObservableList<IAddinAction>Actions { get; }
    }

    

    public interface IIntelliList<T>:IList<T>
    {
        T SelectedItem { get; set; }
    }

    public interface IDynamicRelationshipType
    {
        string Type { get; }
        string Key { get; }
    }
}