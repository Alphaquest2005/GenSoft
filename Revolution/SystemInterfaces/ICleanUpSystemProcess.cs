namespace SystemInterfaces
{
    
    public interface ICleanUpSystemProcess : IProcessSystemMessage
    {
        int ProcessToBeCleanedUpId { get; }
    }
}