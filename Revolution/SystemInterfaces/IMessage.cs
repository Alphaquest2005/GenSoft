using System;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{
    
    public interface IMessage : IEvent
    {
        IDynamicObject Message { get; }
        DateTime MessageDateTime { get; }
    }
}
