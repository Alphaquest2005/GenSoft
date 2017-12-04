namespace SystemInterfaces
{
    public interface IEntityFound : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
