namespace SystemInterfaces
{
    
    public interface ICleanUpSystemProcess : IProcessSystemMessage
    {
        ISystemProcess ProcessToBeCleanedUp { get; }
    }
}