using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
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
    public class FooterViewModelInfo
    {
        public static ViewModelInfo FooterViewModel(int processId)
        {
            return new ViewModelInfo
            (
                processId,
                new ViewInfo("Footer", "", ""),
                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IFooterViewModel, ICurrentEntityChanged>(
                        "Footer-ICurrentEntityChanged",
                        processId,
                        e => e.Entity != null  && e.Entity.Id > 0,
                        new List<Func<IFooterViewModel, ICurrentEntityChanged, bool>>(),
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
                new List<IViewModelEventPublication<IViewModel, IEvent>> { },
                new List<IViewModelEventCommand<IViewModel, IEvent>>
                {


                    new ViewEventCommand<IFooterViewModel, INavigateToView>(
                        key: "NavigateToView",
                        commandPredicate: new List<Func<IFooterViewModel, bool>> {s => s.CurrentEntity.Value != null},
                        subject: s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                        messageData: s => new ViewEventCommandParameter(
                            new object[] {$"{s.CurrentEntity.Value.EntityType.Name}-SummaryListViewModel"},
                            new StateCommandInfo(s.Process.Id,
                                Context.View.Commands.NavigateToView), s.Process,
                            s.Source)),


                },
                typeof(IFooterViewModel),
                typeof(IFooterViewModel), 
                0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ));
        }
    }
}