using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IDynamicEntityType
    {
        string Name { get; }

        string EntitySetName { get; }
        List<IEntityKeyValuePair> Properties { get; }
    }
}