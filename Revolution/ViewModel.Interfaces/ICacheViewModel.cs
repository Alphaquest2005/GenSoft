using System.ComponentModel.Composition;
using SystemInterfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface ICacheViewModel : IViewModel
    {

    }

    public interface ISummaryListViewModel : IViewModel
    {

    }

    public interface IEntityDetailsViewModel : IViewModel
    {

    }

    public interface IEntityCacheViewModel<TEntity> :ICacheViewModel, IEntityListViewModel<TEntity> where TEntity : IEntity//
    {

    }

    public interface IEntityCacheViewModeltest<TEntity> : ICacheViewModel
    {

    }
    public interface IEntityViewCacheViewModel<TView> : ICacheViewModel, IEntityListViewModel<TView> where TView : IEntityView
    {

    }

    public interface ISummaryListViewModel<TView> : ISummaryListViewModel, IEntityListViewModel<TView> where TView : IEntityView
    {
        
    }

    public interface IEntityDetailsViewModel<TView> : IEntityDetailsViewModel, IEntityViewModel<TView> where TView : IEntityView
    {

    }

}

