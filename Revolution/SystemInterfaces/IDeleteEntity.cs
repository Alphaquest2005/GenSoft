namespace SystemInterfaces
{
    
    public interface IDeleteEntity : IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }
}
