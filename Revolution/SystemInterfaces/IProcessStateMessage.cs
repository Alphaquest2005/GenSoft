namespace SystemInterfaces
{
    
    public interface IProcessStateMessage:IEntityRequest
    {
        IProcessStateEntity State { get; }
    }

    
    public interface IUpdateProcessStateList : IEntityRequest 
    {
        IProcessStateList State { get; }
    }
}
