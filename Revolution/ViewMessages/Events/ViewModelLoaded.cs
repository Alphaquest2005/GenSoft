using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelLoaded<, >))]
    public class ViewModelLoaded<TLoadingViewModel,TViewModel> : ProcessSystemMessage, IViewModelLoaded<TLoadingViewModel,TViewModel>
    {
        //occurs when viewmodel loaded in View
        public ViewModelLoaded() { }
        public ViewModelLoaded(TLoadingViewModel loadingViewModel, TViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo,process, source)
        {
            LoadingViewModel = loadingViewModel;
            ViewModel = viewModel;
        }

        public TLoadingViewModel LoadingViewModel { get; }
        public TViewModel ViewModel { get; }

    }

    [Export(typeof(IViewModelIntialized))]
    public class ViewModelIntialized : ProcessSystemMessage, IViewModelIntialized
    {
        //occurs when viewmodel loaded in View
        public ViewModelIntialized() { }
        public ViewModelIntialized(IViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            ViewModel = viewModel;
        }

        public IViewModel ViewModel { get; }

    }
}
