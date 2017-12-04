namespace SystemInterfaces
{
    
    public interface IStateEventInfo : IProcessStateInfo
    {
        new IStateEvent State { get; }
        
    }
}