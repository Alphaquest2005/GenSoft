namespace SystemInterfaces
{
    
    public interface ISystemSource:ISource
    {
        IMachineInfo MachineInfo { get; }
    }
}