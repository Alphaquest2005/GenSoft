using System;

namespace SystemInterfaces
{
    
    public interface IEventPublication
    {
        string Key { get; }
        Type EventType { get; }
    }



 }