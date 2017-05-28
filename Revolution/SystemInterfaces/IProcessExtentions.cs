using System.Collections.Concurrent;

namespace SystemInterfaces
{
    public static class IProcessExtentions
    {
        static ConcurrentDictionary<IProcess, IProcessSource> ProcessSources = new ConcurrentDictionary<IProcess, IProcessSource>();
    }
}