using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IDynamicEntityType
    {
        string Name { get; }
        List<IEntityKeyValuePair> Properties { get; }
    }
}