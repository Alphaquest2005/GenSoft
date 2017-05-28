using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IPatientVisitViewModel : IEntityListViewModel<IPatientVisitInfo>
    {
        IPatientInfo CurrentPatient { get; set; }
    }
}