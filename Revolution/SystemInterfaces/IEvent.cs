namespace SystemInterfaces
{
    
    public interface IEvent
    {
        //ToDo:implement visitor for this
        ISystemSource Source { get; set; }
    }
}