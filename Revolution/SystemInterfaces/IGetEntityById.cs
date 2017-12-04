namespace SystemInterfaces
{
    
    
    public interface IGetEntityById: IEntityRequest
    {
       // void Create(int entityId);
        int EntityId { get; }
    }
}