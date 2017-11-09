using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(ILoadViewModel))]
    public class LoadViewModel : ProcessSystemMessage, ILoadViewModel
    {
        public LoadViewModel() { }
        public LoadViewModel(IViewModelInfo viewModelInfo, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("LoadViewModel", new Dictionary<string, object>() { { "ViewModelInfo", viewModelInfo }}), processInfo, process, source)
        {
            ViewModelInfo = viewModelInfo;

        }

        public IViewModelInfo ViewModelInfo { get;}
    }
}