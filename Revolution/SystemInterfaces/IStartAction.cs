namespace SystemInterfaces
{
    public interface IStartAddin : IProcessSystemMessage
    {
        IAddinAction Action { get; }
        IDynamicEntity Entity { get; }
    }

    public interface IAddinAction
    {
        string Action { get; }
        string Addin { get; }
    }
}