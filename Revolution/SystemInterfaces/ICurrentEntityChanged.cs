namespace SystemInterfaces
{
    
    public interface ICurrentEntityChanged:IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }

    public interface ICurrentApplicationChanged : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }

    public interface IMainEntityChanged : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
