using System.Windows;
using SystemInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface IViewRowStateChanged : IProcessSystemMessage
    {
        IViewModel ViewModel { get; }
        RowState RowState { get; }
    }

    public interface IViewModelStateChanged : IProcessSystemMessage
    {
        IViewModel ViewModel { get; }
        ViewModelState ViewModelState { get; }
    }

    public interface IViewModelVisibilityChanged : IProcessSystemMessage
    {
        IViewModel ViewModel { get; }
        Visibility Visibility { get; }
    }

}
