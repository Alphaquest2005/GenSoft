namespace SystemInterfaces
{
    
    public interface IProcessStateUpddated : IEntityRequest
    {
        IProcessStateMessage StateMessage { get; }
    }
}
