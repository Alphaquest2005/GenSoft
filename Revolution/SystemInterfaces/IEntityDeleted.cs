namespace SystemInterfaces
{
    
    public interface IEntityDeleted: IEntityRequest
    {
        IDynamicEntity Entity { get; }
    }

}
