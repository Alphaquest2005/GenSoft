using System.Collections.Concurrent;

namespace SystemInterfaces
{
    
    public interface IProcess
    {
        int Id { get; }
        ISystemProcess ParentProcess { get; }
        string Name { get; }
        string Description { get; }
        string Symbol { get; }
        IUser User { get; }
        IApplet Applet { get; }
    }

    public interface IApplet
    {
        string Name { get; }
    }

    public interface IDbApplet:IApplet
    {
        string DbName { get; }
    }

    public static class IProcessExtentions
    {
        static ConcurrentDictionary<IProcess, IProcessSource> ProcessSources = new ConcurrentDictionary<IProcess, IProcessSource>();
    }
}
