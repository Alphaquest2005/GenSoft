using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;

namespace ViewMessages
{
    [Export(typeof(IViewModelVisibilityChanged))]
    public class ViewModelVisibilityChanged : ProcessSystemMessage, IViewModelVisibilityChanged
    {
        public IViewModel ViewModel { get; }
        public Visibility Visibility { get; }
        public ViewModelVisibilityChanged() { }
        public ViewModelVisibilityChanged(IViewModel viewModel, Visibility visibility, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ViewModelVisibilityChanged", new Dictionary<string, object>() { { "ViewModel", viewModel }, { "Visibility", visibility } }), processInfo, process, source)
        {
            ViewModel = viewModel;
            Visibility = visibility;
        }
    }
}