using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;

namespace ViewModelInterfaces
{
    public interface IEntityListViewModel<TEntity> : IEntityViewModel<TEntity> where TEntity : IEntityId
    {
        IEntityListViewModel<TEntity> Instance { get; }
        ReactiveProperty<TEntity> CurrentEntity { get; }
        ReactiveProperty<ObservableList<TEntity>> EntitySet { get; }
        ReactiveProperty<ObservableList<TEntity>> SelectedEntities { get; }
        
    }
}