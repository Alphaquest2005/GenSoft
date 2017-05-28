namespace SystemInterfaces
{
    public interface IStateCommand : IState
    {
        IStateEvent ExpectedEvent { get; }
    }
}