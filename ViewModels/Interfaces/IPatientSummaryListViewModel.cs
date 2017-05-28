using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IPatientSummaryListViewModel : IEntityListViewModel<IPatientInfo>
    {
        string Field { get; set; }
        string Value { get; set; }
        
    }
}