using System.ComponentModel.Composition;
using System.Windows;
using SystemInterfaces;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewRowStateChanged))]
    public class ViewRowStateChanged : ProcessSystemMessage, IViewRowStateChanged
    {
        public IViewModel ViewModel { get; }
        public RowState RowState { get; }
        public ViewRowStateChanged() { }
        public ViewRowStateChanged(IViewModel viewModel, RowState rowState, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            ViewModel = viewModel;
            RowState = rowState;
        }
    }

    [Export(typeof(IViewModelVisibilityChanged))]
    public class ViewModelVisibilityChanged : ProcessSystemMessage, IViewModelVisibilityChanged
    {
        public IViewModel ViewModel { get; }
        public Visibility Visibility { get; }
        public ViewModelVisibilityChanged() { }
        public ViewModelVisibilityChanged(IViewModel viewModel, Visibility visibility, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            ViewModel = viewModel;
            Visibility = visibility;
        }
    }

    [Export(typeof(IViewModelStateChanged))]
    public class ViewModelStateChanged : ProcessSystemMessage, IViewModelStateChanged
    {
        public IViewModel ViewModel { get; }
        public ViewModelState ViewModelState { get; }
        
        public ViewModelStateChanged() { }
        public ViewModelStateChanged(IViewModel viewModel, ViewModelState viewModelState, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            ViewModel = viewModel;
            ViewModelState = viewModelState;
        }
    }
}