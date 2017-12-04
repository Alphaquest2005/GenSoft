using System;
using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IRequestComplexEventLog : IProcessSystemMessage
    {
    }

    
    public interface IComplexEventLogCreated : IProcessSystemMessage
    {
        IEnumerable<IComplexEventLog> EventLog { get; }
    }

    public interface IComplexEventLog
    {
        string Operation { get; }
        string Status { get; }
        DateTime Time { get; }
        string SourceGuid { get; }
        string SourceName { get; }
        string Source { get; }
        string ExpectedSource { get; }
        string Message { get; }
        string ProcessInfo { get; }
    }
}
