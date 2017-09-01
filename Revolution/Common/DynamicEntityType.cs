using System.Collections.Generic;
using SystemInterfaces;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
        public static Dictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new Dictionary<string, IDynamicEntityType>();

        
        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, bool isList, bool isParentEntity)
        {
            Name = name;
            Properties = properties;
            IsList = isList;
            IsParentEntity = isParentEntity;
            EntitySetName = entitySetName;
        }

        

        public string Name { get; }
        public bool IsList { get; }
        public string EntitySetName { get; }
        public List<IEntityKeyValuePair> Properties { get; }
        public bool IsParentEntity { get; }
    }
}