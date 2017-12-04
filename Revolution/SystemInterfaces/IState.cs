namespace SystemInterfaces
{
    
    public interface IState
    {
        string Name { get; }
        string Status { get; }
        string Notes { get; }
    }

    
    public interface IStateEvent: IState
    {
        IStateCommand ExpectedCommand { get; }
    }

    
    public interface IStateCommand : IState
    {
       IStateEvent ExpectedEvent { get; }
    }

    
}
