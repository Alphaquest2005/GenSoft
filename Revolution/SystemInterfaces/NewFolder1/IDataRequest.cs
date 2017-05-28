using System;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{
    public interface IEntityRequest:IMessage
    {
        Type ViewType { get; }
    }
}