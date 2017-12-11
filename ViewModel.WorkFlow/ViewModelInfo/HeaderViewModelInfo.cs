using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using JB.Collections.Reactive;
using Reactive.Bindings;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
}

namespace RevolutionData
{
    public class HeaderViewModelInfo
    {
        public static ViewModelInfo HeaderViewModel(int processId)
        {
            return new ViewModelInfo
            (
                processId,
                new ViewInfo("HeaderViewModel", "", ""),
                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IHeaderViewModel, IUpdateProcessStateList>(
                        $"HeaderViewModel-IUpdateProcessStateList",
                        processId,
                        e => e.EntityType.Name == "Application",
                        new List<Func<IHeaderViewModel, IUpdateProcessStateList, bool>>(),
                        (v,e) =>
                        {
                            if (e.State != null && v.Entities.Value != null && v.Entities.Value.SequenceEqual(e.State.EntitySet)) return;
                            v.Entities.Value = new ObservableList<IDynamicEntity>(e.State.EntitySet.ToList());
                        }),
                },
                new List<IViewModelEventPublication<IViewModel, IEvent>> {},
                new List<IViewModelEventCommand<IViewModel, IEvent>> {},
                typeof(IHeaderViewModel),
                typeof(IHeaderViewModel), 0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ));
        }
    }
}