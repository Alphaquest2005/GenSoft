using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelStateChanged))]
    public class ViewModelStateChanged : ProcessSystemMessage, IViewModelStateChanged
    {
        public IViewModel ViewModel { get; }
        public ViewModelState ViewModelState { get; }
        
        public ViewModelStateChanged() { }
        public ViewModelStateChanged(IViewModel viewModel, ViewModelState viewModelState, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewRowStateChanged", new Dictionary<string, object>() { { "ViewModel", viewModel }, { "ViewModelState", viewModelState } }), processInfo, process, source)
        {
            ViewModel = viewModel;
            ViewModelState = viewModelState;
        }
    }
}