using System.ComponentModel.Composition;
using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;

namespace ViewModel.Interfaces
{
    
    public interface IFooterViewModel : IViewModel
    {
        ReactiveProperty<IDynamicEntity> CurrentPatient { get; }
        ReactiveProperty<IDynamicEntity> CurrentPatientVisit { get; }
        ReactiveProperty<IDynamicEntity> CurrentPatientSyntom { get; }
        ReactiveProperty<IDynamicEntity> CurrentInterview { get; }
    }
}