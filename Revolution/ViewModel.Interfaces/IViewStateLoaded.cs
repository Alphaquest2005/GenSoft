using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewStateLoaded<out TViewModel, out TProcessState> : IProcessSystemMessage where TViewModel:IViewModel where TProcessState : IProcessState
    {
        TViewModel ViewModel { get; }
        TProcessState State { get; }
    }
}
