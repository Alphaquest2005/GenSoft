namespace SystemInterfaces
{
    public interface IUpdateProcessStateList<out TEntity> : IMessage where TEntity : IEntityId
    {
        IProcessStateList<TEntity> State { get; }
    }
}