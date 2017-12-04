using System.Collections.Concurrent;

namespace SystemInterfaces
{
    
    public interface IProcess
    {
        int Id { get; }
        int ParentProcessId { get; }
        string Name { get; }
        string Description { get; }
        string Symbol { get; }
        IUser User { get; }
    }

    public static class IProcessExtentions
    {
        static ConcurrentDictionary<IProcess, IProcessSource> ProcessSources = new ConcurrentDictionary<IProcess, IProcessSource>();
    }
}
