namespace SystemInterfaces
{
    
    public interface IDeleteEntity : IEntityRequest
    {
        int EntityId { get; }
    }
}
