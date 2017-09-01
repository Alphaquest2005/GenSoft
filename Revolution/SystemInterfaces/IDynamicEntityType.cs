using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IDynamicEntityType
    {
        string Name { get; }
        bool IsList { get; }
        string EntitySetName { get; }
        List<IEntityKeyValuePair> Properties { get; }
        bool IsParentEntity { get;  }
    }
}