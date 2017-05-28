using Interfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IInterviewListViewModel : IEntityListViewModel<IInterviewInfo>
    {
        string Field { get; set; }
        string Value { get; set; }
        ReactiveProperty<IPatientSyntomInfo> CurrentPatientSyntom { get;  }
        ReactiveProperty<ISyntomMedicalSystemInfo> CurrentMedicalSystem { get; }
        ReactiveProperty<ObservableList<ISyntomMedicalSystemInfo>> Systems { get; }
        ReactiveProperty<IMedicalSystems> SelectedMedicalSystem { get; }
    }
}