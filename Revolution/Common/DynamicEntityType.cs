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
                new List<IDynamicRelationshipType>(), new List<IDynamicRelationshipType>(), null, new ObservableList<IAddinAction>());
        }

       


        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, Dictionary<string, List<dynamic>> calculatedProperties, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties, List<IDynamicRelationshipType> parentTypes, List<IDynamicRelationshipType> childTypes, IDynamicEntityType parentEntityType, ObservableList<IAddinAction> actions)
        {
            Name = name;
            Properties = properties;
            ParentEntityType = parentEntityType;
            Actions = actions;
            CachedProperties = cachedProperties;
            CalculatedProperties = calculatedProperties;
            EntitySetName = entitySetName;
            ChildEntities = new InteliList<IDynamicRelationshipType>(childTypes);
            ParentEntities = new InteliList<IDynamicRelationshipType>(parentTypes);


        }

        

        public string Name { get; }
        public string EntitySetName { get; }
        public List<IEntityKeyValuePair> Properties { get; }
        public Dictionary<string, List<dynamic>> CalculatedProperties { get; }
        public ObservableDictionary<string, Dictionary<int, dynamic>> CachedProperties { get; }
        public IIntelliList<IDynamicRelationshipType> ChildEntities { get; } 
        public IIntelliList<IDynamicRelationshipType> ParentEntities { get; }

        public IDynamicEntityType ParentEntityType { get; }
        public ObservableList<IAddinAction> Actions { get; }

        IDynamicEntityType IDynamicEntityType.NullEntityType()
        {
            return NullEntityType();
        }
    }

    public class DynamicRelationshipType: IDynamicRelationshipType
    {
        public DynamicRelationshipType(string type, string key, string ordinality)
        {
            Type = type;
            Key = key;
            Ordinality = ordinality;
        }

        public string Type { get; }
        public string Key { get; }
        public string Ordinality { get; }
    }

}