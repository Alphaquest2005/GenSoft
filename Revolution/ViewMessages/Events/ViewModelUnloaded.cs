using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelEvent<>))]
    public class ViewModelUnloaded<TViewModel> : ProcessSystemMessage, IViewModelEvent<TViewModel>
    {
        public ViewModelUnloaded() { }
        public ViewModelUnloaded(TViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewModelUnloaded", new Dictionary<string, object>() { { "ViewModel", viewModel } }), processInfo, process, source)
        {
            ViewModel = viewModel;
        }

        public TViewModel ViewModel { get; }
    }
}