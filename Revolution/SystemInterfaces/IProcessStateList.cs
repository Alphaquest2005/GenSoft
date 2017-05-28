using System.Collections.Generic;

namespace SystemInterfaces
{
    public interface IProcessStateList<out TEntity> : IProcessState<TEntity> where TEntity : IEntityId
    {
        IEnumerable<TEntity> EntitySet { get; }
        IEnumerable<TEntity> SelectedEntities { get; }
        
    }
}