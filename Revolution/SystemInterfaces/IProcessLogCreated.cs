using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IProcessLogCreated:IProcessSystemMessage
    {
        IEnumerable<IComplexEventLog> EventLogs { get; }
    }
}
