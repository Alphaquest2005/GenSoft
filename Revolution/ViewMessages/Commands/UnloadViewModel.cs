using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IUnloadViewModel))]
    public class UnloadViewModel : ProcessSystemMessage, IUnloadViewModel
    {
        public UnloadViewModel() { }
        public UnloadViewModel(IViewModelInfo viewModelInfo, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("UnloadViewModel", new Dictionary<string, object>() { { "ViewModelInfo", viewModelInfo } }), processInfo, process, source)
        {
            ViewModelInfo = viewModelInfo;

        }

        public IViewModelInfo ViewModelInfo { get; }
    }
}