namespace SystemInterfaces
{
    
    public interface ICreateEntity:IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
