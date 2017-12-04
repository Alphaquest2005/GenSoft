namespace SystemInterfaces
{
    
    public interface IEntityCreated: IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
