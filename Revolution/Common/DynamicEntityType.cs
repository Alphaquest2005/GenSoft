using System.Collections.Generic;
using SystemInterfaces;
using JB.Collections.Reactive;
using Utilities;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
      
        public static IDynamicEntityType NullEntityType()
        {
            return new DynamicEntityType("NullEntity", "NullEntitySet", new List<IEntityKeyValuePair>(),
                new Dictionary<string, List<dynamic>>(), new ObservableDictionary<string, Dictionary<int, dynamic>>(),
                new ObservableDictionary<string, string>(), null);
        }



        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, Dictionary<string, List<dynamic>> calculatedProperties, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties, ObservableDictionary<string, string> propertyParentEntityType, IDynamicEntityType parentEntityType)
        {
            Name = name;
            Properties = properties;
            PropertyParentEntityType = propertyParentEntityType;
            ParentEntityType = parentEntityType;
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

        public IDynamicEntityType ParentEntityType { get; }

        IDynamicEntityType IDynamicEntityType.NullEntityType()
        {
            return NullEntityType();
        }
    }

}