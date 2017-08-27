using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using JB.Collections.Reactive;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewMessages;
using ViewModel.Interfaces;

namespace RevolutionData
{
}

namespace RevolutionData
{
    public class HeaderViewModelInfo
    {
        public static readonly ViewModelInfo HeaderViewModel = new ViewModelInfo
            (
            3,
            new ViewInfo("HeaderViewModel", "", ""),
            new List<IViewModelEventSubscription<IViewModel, IEvent>>
            {
                new ViewEventSubscription<IHeaderViewModel, ICurrentEntityChanged>(
                    3,
                    e => e.Entity != null,
                    new List<Func<IHeaderViewModel, ICurrentEntityChanged, bool>>(),
                    (v, e) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var res = v.Entities.Value.ToList();
                            var existingEntity = res.FirstOrDefault(x => x.EntityType.Name == e.EntityType.Name);
                            if (existingEntity == null)
                            {
                                v.Entities.Value.Add(e.Entity);
                                v.Entities.Value.Reset();
                            }
                            else
                            {
                                var idx = res.IndexOf(existingEntity);
                                res[idx] = e.Entity;
                                v.Entities.Value = new ObservableList<IDynamicEntity>(res);
                            }
                        });



                    }),


            },
            new List<IViewModelEventPublication<IViewModel, IEvent>>{},
            new List<IViewModelEventCommand<IViewModel,IEvent>>
            {


                new ViewEventCommand<IHeaderViewModel, INavigateToView>(
                    key:"NavigateToView",
                    commandPredicate:new List<Func<IHeaderViewModel, bool>>{},
                    subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                    messageData: s =>
                    {
                        return new ViewEventCommandParameter(
                            new object[] {$"{s.CurrentEntity.Value.EntityType.Name}-SummaryListViewModel" },
                            new StateCommandInfo(s.Process.Id,
                                Context.View.Commands.NavigateToView), s.Process,
                            s.Source);
                    }),
                
            },
            typeof(IHeaderViewModel),
            typeof(IHeaderViewModel), 0);
    }
}