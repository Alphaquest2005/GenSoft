namespace SystemInterfaces
{
    public interface IDomainMessage : IProcessSystemMessage
    {
        string Type { get; }
        IDynamicEntity Entity { get; }
    }
}
