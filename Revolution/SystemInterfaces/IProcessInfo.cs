using System;

namespace SystemInterfaces
{
    
    public interface ISystemProcessInfo
    {
        int Id { get; }
        ISystemProcess ParentProcess { get; }
        string Name { get; }
        string Description { get; }
        string Symbol { get; }
        string UserId { get; }
        IApplet Applet { get; }
    }

    public interface ISystemProcessInfo<TEntity>:ISystemProcessInfo where TEntity:IEntityId
    {
      Type EntityType { get; }
    }
}