using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewStateLoaded<,>))]
    public class ViewStateLoaded<TViewModel,TProcessState>: ProcessSystemMessage, IViewStateLoaded<TViewModel,TProcessState> where TProcessState : IProcessState where TViewModel : IViewModel
    {
        public ViewStateLoaded() { }
        public ViewStateLoaded(TViewModel viewModel, TProcessState state, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            :base(new DynamicObject("ViewStateLoaded", new Dictionary<string, object>() { { "ViewModel", viewModel }, { "State", state } }), processInfo, process, source)
        {
            ViewModel = viewModel;
            State = state;
        }

        public TViewModel ViewModel { get; }
        public TProcessState State { get; }
    }
}