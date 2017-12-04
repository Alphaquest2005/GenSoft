using System;

namespace SystemInterfaces
{
    public interface IProcessExpectedEvent
    {
        string Key { get; }
        Func<IProcessSystemMessage, bool> EventPredicate { get; }
        Type EventType { get; }
        int ProcessId { get; }

        IProcessStateInfo ProcessInfo { get; }
        ISourceType ExpectedSourceType { get; }

    }
}
