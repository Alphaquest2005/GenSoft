using System;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{

    public interface IEntityRequest:IProcessSystemMessage
    {
        string EntityType { get; }
    }
}