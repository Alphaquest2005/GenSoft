namespace SystemInterfaces
{
    public interface IEntityUpdated : IEntityRequest 
    {
        IDynamicEntity Entity { get; }
    }
}
