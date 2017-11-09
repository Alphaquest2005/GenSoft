using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelCreated<>))]
    public class ViewModelCreated<TViewModel> : ProcessSystemMessage, IViewModelCreated<TViewModel> where TViewModel:IViewModel
    {
        public ViewModelCreated() { }
        [ImportingConstructor]
        public ViewModelCreated(TViewModel viewModel, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewModelCreated", new Dictionary<string, object>() { { "ViewModel", viewModel } }), processInfo, process, source)
        {
            ViewModel = viewModel;
        }

        public TViewModel ViewModel { get; }

    }
}