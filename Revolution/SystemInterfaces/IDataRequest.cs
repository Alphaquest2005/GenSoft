namespace SystemInterfaces
{

    public interface IEntityRequest:IProcessSystemMessage
    {
        IDynamicEntityType EntityType { get; }
    }
}