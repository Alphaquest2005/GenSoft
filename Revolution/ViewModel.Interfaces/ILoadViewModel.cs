using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface ILoadViewModel : IProcessSystemMessage
    {
        IViewModelInfo ViewModelInfo { get; }
    }
}
