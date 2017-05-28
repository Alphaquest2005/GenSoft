namespace SystemInterfaces
{
    public interface IStateEvent: IState
    {
        IStateCommand ExpectedCommand { get; }
    }
}