using System.Collections.Generic;
using SystemInterfaces;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
        public static Dictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new Dictionary<string, IDynamicEntityType>();

        
        public DynamicEntityType(string name, List<IEntityKeyValuePair> properties)
        {
            Name = name;
            Properties = properties;
        }

        

        public string Name { get; }
        public List<IEntityKeyValuePair> Properties { get; }
    }
}