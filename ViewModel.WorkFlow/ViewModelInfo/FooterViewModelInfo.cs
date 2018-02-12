using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using JB.Collections.Reactive;
using Process.WorkFlow;
using Reactive.Bindings;
using RevolutionData.Context;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
}

namespace RevolutionData
{
    public class FooterViewModelInfo
    {
        public static ViewModelInfo FooterViewModel()
        {
            return new ViewModelInfo
            (
                Processes.IntialSystemProcess,
                new ViewInfo("Footer", "", ""),
                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IFooterViewModel, ICurrentEntityChanged>(
                        "Footer-ICurrentEntityChanged",
                        Processes.IntialSystemProcess,
                        e => e.Entity != null  && e.Entity.Id > 0,
                        new List<Func<IFooterViewModel, ICurrentEntityChanged, bool>>(),
                        (v, e) =>
                        {
                            
                            if (Application.Current == null)
                            {
                                OnCurrentEntityChanged(v, e);
                            }
                            else
                            {
                                Application.Current.Dispatcher.Invoke(() => { OnCurrentEntityChanged(v, e); });

                            }

                        }, new RevolutionEntities.Process.StateEventInfo( Processes.IntialSystemProcess,  Context.ViewModel.Events.CurrentEntityChanged, Guid.NewGuid())),

                    new ViewEventSubscription<IFooterViewModel, ICleanUpSystemProcess>(
                        "Footer-ICleanUpSystemProcess",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IFooterViewModel, ICleanUpSystemProcess, bool>> { },
                        (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.Entities.Value.Clear();
                                s.Entities.Value.Reset();
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    s.Entities.Value.Clear();
                                    s.Entities.Value.Reset();
                                }));
                            }
                        }, new StateCommandInfo(Processes.IntialSystemProcess, CommandFunctions.UpdateCommandData(Processes.IntialSystemProcess.Name,RevolutionData.Context.Process.Commands.CleanUpProcess), Guid.NewGuid())),

                    new ViewEventSubscription<IFooterViewModel, ICurrentApplicationChanged>(
                        "ScreenViewModel-ICleanUpSystemProcess",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IFooterViewModel, ICurrentApplicationChanged, bool>> { },
                        (s, e) =>
                        {

                            if (Application.Current == null)
                            {
                                s.Entities.Value.Clear();
                                s.Entities.Value.Reset();
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    s.Entities.Value.Clear();
                                    s.Entities.Value.Reset();
                                }));
                            }
                        }, new StateEventInfo(Processes.IntialSystemProcess, RevolutionData.Context.Process.Events.CurrentApplicationChanged, Guid.NewGuid())),
                },
                new List<IViewModelEventPublication<IViewModel, IEvent>> { },
                new List<IViewModelEventCommand<IViewModel, IEvent>>
                {


                    new ViewEventCommand<IFooterViewModel, INavigateToView>(
                        key: "NavigateToView",
                        commandPredicate: new List<Func<IFooterViewModel, bool>> {s => s.CurrentEntity?.Value != null},
                        subject: s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                        messageData: s => new ViewEventCommandParameter(
                            new object[] {$"{s.CurrentEntity.Value?.EntityType.Name}-SummaryListViewModel"},
                            new StateCommandInfo(s.Process,
                                Context.View.Commands.NavigateToView), s.Process,
                            s.Source)),


                },
                typeof(IFooterViewModel),
                typeof(IFooterViewModel), 
                0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ),
                new List<IViewModelInfo>());
        }

        private static void OnCurrentEntityChanged(IFooterViewModel v, ICurrentEntityChanged e)
        {
            var res = v.Entities.Value.ToList();
            var existingEntity =
                res.FirstOrDefault(x => x.EntityType.Name == e.EntityType.Name);
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
        }
    }
}