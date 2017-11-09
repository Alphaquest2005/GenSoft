using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
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
        public ViewRowStateChanged(IViewModel viewModel, RowState rowState, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewRowStateChanged", new Dictionary<string, object>() { { "ViewModel", viewModel }, { "RowState", rowState } }), processInfo, process, source)
        {
            ViewModel = viewModel;
            RowState = rowState;
        }
    }
}