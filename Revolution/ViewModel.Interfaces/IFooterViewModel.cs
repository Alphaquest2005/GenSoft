using System.ComponentModel.Composition;
using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;

namespace ViewModel.Interfaces
{
    
    public interface IFooterViewModel : IViewModel
    {
        ObservableList<IDynamicEntity> Entities { get; }

    }
}