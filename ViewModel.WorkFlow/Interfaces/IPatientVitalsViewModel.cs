using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IPatientVitalsViewModel : IEntityViewModel<IPatientVitalsInfo>
    {
        IPatientInfo CurrentPatient { get; set; }

    }
}