using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace RevolutionData
{
    public class ScreenViewModelInfo
    {
        public static ViewModelInfo ScreenViewModel(int processId)
        {
            return new ViewModelInfo
            (
                processId,
                new ViewInfo("ScreenViewModel", "", ""),

                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IScreenModel, INavigateToView>(
                        "ScreenViewModel-ICurrentEntityChanged",
                        processId,
                        e => e != null,
                        new List<Func<IScreenModel, INavigateToView, bool>> { },
                        (s, e) =>
                        {
                            s.Slider.BringIntoView(e.View);
                        }),

                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged", processId, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(IHeaderViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.HeaderViewModels.Add(e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                    new Action(() => s.HeaderViewModels.Add(e.ViewModel)));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged",
                        processId,
                        e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(ILeftViewModel)
                        },
                        (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.LeftViewModels.Add(e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                    new Action(() => s.LeftViewModels.Add(e.ViewModel)));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged", processId, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(IRightViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.RightViewModels.Add(e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                    new Action(() => s.RightViewModels.Add(e.ViewModel)));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged",
                        processId, e => e != null, new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(IFooterViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.FooterViewModels.Add(e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                    new Action(() => s.FooterViewModels.Add(e.ViewModel)));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged", processId, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(IBodyViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.BodyViewModels.Insert(e.ViewModel.Priority, e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    var last = s.BodyViewModels.FirstOrDefault(x => x.Priority > e.ViewModel.Priority);
                                    if (last == null)
                                    {
                                        s.BodyViewModels.Add(e.ViewModel);
                                    }
                                    else
                                    {
                                        s.BodyViewModels.Insert(s.BodyViewModels.IndexOf(last), e.ViewModel);
                                    }
                                }));
                            }
                        }),

                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-ICurrentEntityChanged", processId, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => s.Process.Id != e.ViewModel.Process.Id &&
                                      e.ViewModel.Orientation == typeof(ICacheViewModel)
                        }, (s, e) =>
                        {
                            var em = (e.ViewModel as IEntityViewModel);
                            var viewName = em != null
                                ? (em.ViewInfo as IEntityViewInfo).EntityType.Name
                                : e.ViewModel.ViewModelType.Name;
                            if (Application.Current == null)
                            {
                                
                                s.CacheViewModels.Add(viewName,e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                    new Action(() => s.CacheViewModels.Add(viewName,e.ViewModel)));
                            }
                        }),

                    new ViewEventSubscription<IScreenModel, ICleanUpSystemProcess>(
                        "ScreenViewModel-ICurrentEntityChanged",
                        processId,
                        e => e != null,
                        new List<Func<IScreenModel, ICleanUpSystemProcess, bool>> { },
                        (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                ClearScreenModels(s, e);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    ClearScreenModels(s, e);
                                }));
                            }
                        }),
                },
                new List<IViewModelEventPublication<IViewModel, IEvent>>
                {
                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelBody",
                        subject: v => v.BodyViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.BodyViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.BodyViewModels.Last()},
                            new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                            s.BodyViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelLeft",
                        subject: v => v.LeftViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.LeftViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.LeftViewModels.Last()},
                            new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                            s.LeftViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelHeader",
                        subject: v => v.HeaderViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.HeaderViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.HeaderViewModels.Last()},
                            new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                            s.HeaderViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelRight",
                        subject: v => v.RightViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.RightViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.RightViewModels.Last()},
                            new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                            s.RightViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelFooter",
                        subject: v => v.FooterViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.FooterViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.FooterViewModels.Last()},
                            new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                            s.FooterViewModels.Last().Process, s.Source)),

                    //new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                    //    key: "ScreenModelCache",
                    //    subject: v => v.CacheViewModels.CollectionChanges,
                    //    subjectPredicate: new List<Func<IScreenModel, bool>>
                    //    {
                    //        v => v.CacheViewModels.LastOrDefault() != null
                    //    },
                    //    messageData: s => new ViewEventPublicationParameter(new object[] {s, s.CacheViewModels.Last()},
                    //        new StateEventInfo(s.Process.Id, Context.ViewModel.Events.ViewModelLoaded),
                    //        s.CacheViewModels.Last().Process, s.Source)),


                },
                new List<IViewModelEventCommand<IViewModel, IEvent>>(),
                typeof(IScreenModel),
                typeof(IBodyViewModel),
                0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ));
        }

        private static void ClearScreenModels(IScreenModel s, ICleanUpSystemProcess e)
        {
            s.BodyViewModels.RemoveRange(
                s.BodyViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUpId));
            s.BodyViewModels.Reset();
            s.LeftViewModels.RemoveRange(
                s.LeftViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUpId));
            s.LeftViewModels.Reset();
            s.HeaderViewModels.RemoveRange(
                s.HeaderViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUpId));
            s.HeaderViewModels.Reset();
            s.RightViewModels.RemoveRange(
                s.RightViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUpId));
            s.RightViewModels.Reset();
            s.FooterViewModels.RemoveRange(
                s.FooterViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUpId));
            s.FooterViewModels.Reset();

            foreach (var itm in s.CacheViewModels.Where(x => x.Value.Process.Id == e.ProcessToBeCleanedUpId).ToList())
            {
                s.CacheViewModels.Remove(itm.Key);
            }
        }
    }
}