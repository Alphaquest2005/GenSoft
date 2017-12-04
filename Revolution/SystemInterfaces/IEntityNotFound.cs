namespace SystemInterfaces
{
    public interface IEntityNotFound: IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
