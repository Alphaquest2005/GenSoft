using System;

namespace SystemInterfaces
{
    
    public interface IMessage : IEvent
    {
        IDynamicObject Message { get; }
        DateTime MessageDateTime { get; }
    }
}
