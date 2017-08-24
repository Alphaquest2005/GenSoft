using System;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{

    public interface IEntityRequest:IProcessSystemMessage
    {
        IDynamicEntityType EntityType { get; }
    }
}