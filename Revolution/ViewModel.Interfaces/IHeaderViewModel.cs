using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;

namespace ViewModel.Interfaces
{
    
    public interface IHeaderViewModel: IViewModel
    {
        ReactiveProperty<ObservableList<IDynamicEntity>> Entities { get; }

        ReactiveProperty<IDynamicEntity> CurrentEntity { get; }
    }
}
