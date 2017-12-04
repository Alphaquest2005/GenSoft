using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelLoaded<, >))]
    public class ViewModelLoaded<TLoadingViewModel,TViewModel> : ProcessSystemMessage, IViewModelLoaded<TLoadingViewModel,TViewModel>
    {
        //occurs when viewmodel loaded in View
        public ViewModelLoaded() { }
        public ViewModelLoaded(TLoadingViewModel loadingViewModel, TViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewModelLoaded", new Dictionary<string, object>() { { "ViewModel", viewModel }, { "LoadingViewModel", loadingViewModel } }), processInfo, process, source)
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
        public ViewModelIntialized(IViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewModelIntialized", new Dictionary<string, object>() { { "ViewModel", viewModel } }), processInfo, process, source)
        {
            ViewModel = viewModel;
        }

        public IViewModel ViewModel { get; }

    }
}
