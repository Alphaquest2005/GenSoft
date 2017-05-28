using Interfaces;
using Reactive.Bindings;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IPatientSyntomViewModel : IEntityListViewModel<IPatientSyntomInfo>
    {
        ReactiveProperty<IPatientVisitInfo> CurrentPatientVisit { get; }
    }
}