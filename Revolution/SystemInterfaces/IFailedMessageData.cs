namespace SystemInterfaces
{
    public interface IFailedMessageData : IProcessSystemMessage
    {
        dynamic Data { get; set; }
    }
}
