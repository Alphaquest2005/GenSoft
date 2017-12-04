namespace SystemInterfaces
{
    
    public interface IStateCommandInfo : IProcessStateInfo
    {
        new IStateCommand State { get; }
       
    }
}