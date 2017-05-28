using SystemInterfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IEntityCacheViewModel<TEntity> :ICacheViewModel, IEntityListViewModel<TEntity> where TEntity : IEntity//
    {

    }
}