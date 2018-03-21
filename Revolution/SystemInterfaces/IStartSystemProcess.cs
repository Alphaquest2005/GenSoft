namespace SystemInterfaces
{
    
    public interface IStartSystemProcess : IProcessSystemMessage
    {
        int ProcessToBeStartedId { get; }
    }
}
