using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SystemInterfaces;
using Common;
using Common.Annotations;
using JB.Collections.Reactive;
using Reactive.Bindings;

using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;


namespace Core.Common.UI
{
    public abstract class BaseViewModel<TViewModel> : IViewModel, INotifyPropertyChanged
    {
        public ISystemSource Source { get; }

        public BaseViewModel(){}

        protected BaseViewModel(ISystemProcess process, IViewInfo viewInfo, IReadOnlyList<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, IReadOnlyList<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, IReadOnlyList<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            Source = new Source(Guid.NewGuid(), "ViewModel:" + typeof(TViewModel).GetFriendlyName(), new SourceType(typeof(BaseViewModel<TViewModel>)),process,process.MachineInfo);
            Process = process;
            EventSubscriptions = eventSubscriptions;
            EventPublications = eventPublications;
            CommandInfo = commandInfo;
            Orientation = orientation;
            Priority = priority;
            ViewInfo = viewInfo;
            ViewModelType = typeof(TViewModel);
            
            RowState = new ReactiveProperty<RowState>(SystemInterfaces.RowState.Loaded);

        }

        
        public IReadOnlyList<IViewModelEventSubscription<IViewModel, IEvent>> EventSubscriptions { get;}
        public IReadOnlyList<IViewModelEventPublication<IViewModel, IEvent>> EventPublications { get; }
        public IReadOnlyList<IViewModelEventCommand<IViewModel, IEvent>> CommandInfo { get; }
        public ReactiveProperty<RowState> RowState { get; }
        public Type Orientation { get; }
        public Type ViewModelType { get; }
        public int Priority { get; }

        public ObservableList<IViewModel> ViewModels { get; } = new ObservableBindingList<IViewModel>();
        public ReactiveProperty<dynamic> SelectedViewModel { get; } = new ReactiveProperty<dynamic>();
        public ReactiveProperty<dynamic> ViewModelState { get; } = new ReactiveProperty<dynamic>(SystemInterfaces.ViewModelState.NotIntialized);
        public ReactiveProperty<dynamic> Visibility { get; } = new ReactiveProperty<dynamic>(System.Windows.Visibility.Collapsed);

        public Dictionary<string, ReactiveCommand<IViewModel>> Commands { get; } = new Dictionary<string, ReactiveCommand<IViewModel>>();

        public IViewInfo ViewInfo { get; }
        public ISystemProcess Process { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}