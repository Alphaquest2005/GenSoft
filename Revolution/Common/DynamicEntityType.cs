using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemInterfaces;
using JB.Collections.Reactive;
using Utilities;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
        public static ConcurrentDictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new ConcurrentDictionary<string, IDynamicEntityType>();

        public static IDynamicEntityType NullEntityType()
        {
            return new DynamicEntityType("NullEntity", "NullEntitySet", new List<IEntityKeyValuePair>(),
                new Dictionary<string, List<dynamic>>(), new ObservableDictionary<string, Dictionary<int, dynamic>>(),
                new ObservableDictionary<string, string>());
        }


        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, Dictionary<string, List<dynamic>> calculatedProperties, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties, ObservableDictionary<string, string> propertyParentEntityType)
        {
            Name = name;
            Properties = properties;
            PropertyParentEntityType = propertyParentEntityType;
            CachedProperties = cachedProperties;
            CalculatedProperties = calculatedProperties;
            EntitySetName = entitySetName;
        }

        

        public string Name { get; }
        public string EntitySetName { get; }
        public List<IEntityKeyValuePair> Properties { get; }
        public Dictionary<string, List<dynamic>> CalculatedProperties { get; }
        public ObservableDictionary<string, Dictionary<int, dynamic>> CachedProperties { get; }
        public ObservableDictionary<string, string> PropertyParentEntityType { get; }
        public IIntelliList<IDynamicEntityType> ChildEntities { get; } = new InteliList<IDynamicEntityType>();
        public IIntelliList<IDynamicEntityType> ParentEntities { get; } = new InteliList<IDynamicEntityType>();
        IDynamicEntityType IDynamicEntityType.NullEntityType()
        {
            return NullEntityType();
        }
    }

}