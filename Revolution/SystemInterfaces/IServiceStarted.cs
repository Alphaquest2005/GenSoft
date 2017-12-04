namespace SystemInterfaces
{
    public interface IServiceStarted<out TService> : IProcessSystemMessage
    {
        TService Service { get; }
    }
    
}
