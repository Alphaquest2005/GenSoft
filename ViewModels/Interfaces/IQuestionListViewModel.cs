using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IQuestionListViewModel : IEntityListViewModel<IQuestionInfo>
    {
        IInterviewInfo CurrentInterview { get; set; }
    }
}