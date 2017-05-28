using Interfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IQuestionaireViewModel : IEntityListViewModel<IResponseOptionInfo>
    {
        ObservableList<IQuestionResponseOptionInfo> Questions { get; set; }
        IPatientVisitInfo CurrentPatientVisit { get; set; }
        IPatientSyntomInfo CurrentPatientSyntom { get; set; }
        ReactiveProperty<IQuestionResponseOptionInfo> CurrentQuestion { get; }
        ReactiveProperty<IQuestionResponseTypes> DataType { get; }
    }
}