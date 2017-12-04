using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface INavigateToView:IProcessSystemMessage
    {
        string View { get; }
    }
}
