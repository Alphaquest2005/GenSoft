using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IComplexEventLogCreated : IMessage
    {
        IEnumerable<IComplexEventLog> EventLog { get; }
    }
}