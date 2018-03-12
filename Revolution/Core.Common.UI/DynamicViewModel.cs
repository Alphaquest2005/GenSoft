using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using SystemInterfaces;
using Common;
using Common.Dynamic;
using JB.Collections.Reactive;
using Process.WorkFlow;
using Reactive.Bindings;

using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace Core.Common.UI
{


    public class DynamicViewModel<TViewModel> : Expando, IDynamicViewModel<TViewModel> where TViewModel:IViewModel
    {
        public DynamicViewModel(){}

        public ISystemSource Source { get; private set; }
        public TViewModel ViewModel { get; private set; }

   
        public DynamicViewModel(TViewModel viewModel) : base(viewModel)
        {
            Contract.Requires(viewModel != null);
            ViewInfo = viewModel.ViewInfo;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            Source = new Source(Guid.NewGuid(), "DynamicViewModel:" + typeof(TViewModel).GetFriendlyName(), new SourceType(typeof(DynamicViewModel<TViewModel>)),viewModel.Process,viewModel.Process.MachineInfo);
            CommandInfo = viewModel.CommandInfo;
            Commands = viewModel.Commands;
            EventPublications = viewModel.EventPublications;
            EventSubscriptions = viewModel.EventSubscriptions;
            Process = viewModel.Process;
            ViewModel = (TViewModel) base.Instance;
            Orientation = viewModel.Orientation;
            ViewModelType = typeof (TViewModel);
            //RowState = viewModel.RowState;
            Priority = viewModel.Priority;

            ViewModels.CollectionChanges.Subscribe(x =>
            {
                NotifyPropertyChanged(nameof(ExtensionViewModels));
                
            });

            EventAggregator.EventMessageBus.Current.GetEvent<ICleanUpSystemProcess>(
                new StateEventInfo(Process,
                    RevolutionData.Context.EventFunctions.UpdateEventData(Process.Name,
                        RevolutionData.Context.Process.Events.ProcessCleanedUp), Guid.NewGuid()), Source)
                        .Where(x => x.ProcessToBeCleanedUp.Id == Process.Id && Process.Id > Processes.IntialSystemProcess.Id).Subscribe(x => CleanUpView());
            
            

        }

        private void CleanUpView()
        {
            ViewInfo = null;
            CommandInfo = null;
            Commands = null;
            EventPublications = null;
            EventSubscriptions = null;
            
            ViewModel = default(TViewModel);
            Orientation = null;
            ViewModelType = null;
            RowState = null;
            SelectedViewModel = null;
            ViewModelState = null;
           // Visibility = null;
            ViewModels = null;
            
        }


        public List<IEntityViewModel> ExtensionViewModels => ViewModels?.Cast<IEntityViewModel>().Where(x => x.ViewInfo?.Ordinality == EntityRelationshipOrdinality.One).ToList();
        public List<IEntityViewModel> ChildEntityViewModels => ViewModels?.Cast<IEntityViewModel>().Where(x => x.ViewInfo?.Ordinality == EntityRelationshipOrdinality.Many).ToList();

        public ReactiveProperty<RowState> RowState { get; private set; } = new ReactiveProperty<RowState>(SystemInterfaces.RowState.Loaded);
        public ObservableList<IViewModel> ViewModels { get; private set; } = new ObservableBindingList<IViewModel>();
        public ReactiveProperty<dynamic> SelectedViewModel { get; private set; } = new ReactiveProperty<dynamic>();
        public ReactiveProperty<dynamic> ViewModelState { get; private set; } = new ReactiveProperty<dynamic>(SystemInterfaces.ViewModelState.NotInitialized);
        public ReactiveProperty<dynamic> Visibility { get; private set; } = new ReactiveProperty<dynamic>(System.Windows.Visibility.Collapsed);

       

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            base.InvokeMethod(this, "GetValue", new object[] { binder.Name }, out result);
            if (result != null) return true;
            var res = base.TryGetMember(binder, out result);
            if(res == false) throw new InvalidOperationException($"Property not found{binder.Name}");
            return true;
            
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            object result = null;
            base.InvokeMethod(this, "SetValue", new object[] { value, binder.Name }, out result);
            if (result != null) return true;
            var res = base.TrySetMember(binder, value);
            if(res == false) throw new InvalidOperationException($"Property not found{binder.Name}");
            return true;
        }

        public void NotifyPropertyChanged(string property)
        {
            this.OnPropertyChanged(property);
        }

        public string ViewName { get; private set; } 
        public string ViewSymbol { get; private set; }
        public string ViewDescription { get; private set; }
        public IViewInfo ViewInfo { get; private set; }
        public ISystemProcess Process { get; private set; }
        public IReadOnlyList<IViewModelEventSubscription<IViewModel, IEvent>> EventSubscriptions { get; private set; }
        public IReadOnlyList<IViewModelEventPublication<IViewModel, IEvent>> EventPublications { get; private set; }
        public Dictionary<string, ReactiveCommand<IViewModel>> Commands { get; private set; }
        public IReadOnlyList<IViewModelEventCommand<IViewModel, IEvent>> CommandInfo { get; private set; }
        public Type Orientation { get; private set; }
        public Type ViewModelType { get; private set; }
        public int Priority { get;  }
    }
}
