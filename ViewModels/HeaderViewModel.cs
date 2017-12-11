using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Core.Common.UI;
using GenSoft.Entities;
using JB.Collections.Reactive;
using Reactive.Bindings;


using ViewModel.Interfaces;
using Type = System.Type;

namespace ViewModels
{

    [Export(typeof(IHeaderViewModel))]
    public class HeaderViewModel : DynamicViewModel<ObservableViewModel>, IHeaderViewModel
    {
        [ImportingConstructor]
        public HeaderViewModel(ISystemProcess process, IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, Type orientation, int priority) : base(new ObservableViewModel(viewInfo,eventSubscriptions, eventPublications, commandInfo, process, orientation, priority))
        {
            this.WireEvents();

        }

        public ReactiveProperty<ObservableList<IDynamicEntity>> Entities { get; } = new ReactiveProperty<ObservableList<IDynamicEntity>>(new ObservableList<IDynamicEntity>());
        public ReactiveProperty<IDynamicEntity> CurrentEntity { get; } = new ReactiveProperty<IDynamicEntity>();

       
    }
}
