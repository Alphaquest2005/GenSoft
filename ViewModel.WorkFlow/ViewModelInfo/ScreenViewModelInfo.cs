using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using SystemInterfaces;
using Common;
using Process.WorkFlow;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
    public class ScreenViewModelInfo
    {
        public ViewModelInfo ScreenViewModel()
        {
            
            return new ViewModelInfo
            (
                Processes.IntialSystemProcess,
                new ViewInfo("ScreenViewModel", "", ""),

                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IScreenModel, IViewModelVisibilityChanged>(
                        "ScreenViewModel-IViewModelVisibilityChanged",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IScreenModel, IViewModelVisibilityChanged, bool>> { },
                        (s, e) =>
                        {
                            foreach (var itm in s.BodyViewModels.Where(x => x.ViewInfo.Name == e.ViewModel.ViewInfo.Name))
                            {
                                itm.Visibility.Value = e.Visibility;
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, INavigateToView>(
                        "ScreenViewModel-INavigateToView",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IScreenModel, INavigateToView, bool>> { },
                        (s, e) =>
                        {
                            s.Slider.BringIntoView(e.View);
                        }),

                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IHeaderViewModel>>(
                        "ScreenViewModel-IViewModelCreated<IHeaderViewModel>", Processes.IntialSystemProcess, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IHeaderViewModel>, bool>>
                        {
                            (s, e) => e.ViewModel.Orientation == typeof(IHeaderViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.HeaderViewModels.Insert(e.ViewModel.Priority, e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    var last = s.HeaderViewModels.FirstOrDefault(x => x.Priority > e.ViewModel.Priority);
                                    if (last == null)
                                    {
                                        s.HeaderViewModels.Add(e.ViewModel);
                                    }
                                    else
                                    {
                                        s.HeaderViewModels.Insert(s.HeaderViewModels.IndexOf(last), e.ViewModel);
                                    }
                                }));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-IViewModelCreated<ILeftViewModel>",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => e.ViewModel.Orientation == typeof(ILeftViewModel)
                        },
                        (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.LeftViewModels.Insert(e.ViewModel.Priority, e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    var last = s.LeftViewModels.FirstOrDefault(x => x.Priority > e.ViewModel.Priority);
                                    if (last == null)
                                    {
                                        s.LeftViewModels.Add(e.ViewModel);
                                    }
                                    else
                                    {
                                        s.LeftViewModels.Insert(s.LeftViewModels.IndexOf(last), e.ViewModel);
                                    }
                                }));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-IViewModelCreated<IRightViewModel>", Processes.IntialSystemProcess, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => e.ViewModel.Orientation == typeof(IRightViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.RightViewModels.Insert(e.ViewModel.Priority, e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    var last = s.RightViewModels.FirstOrDefault(x => x.Priority > e.ViewModel.Priority);
                                    if (last == null)
                                    {
                                        s.RightViewModels.Add(e.ViewModel);
                                    }
                                    else
                                    {
                                        s.RightViewModels.Insert(s.RightViewModels.IndexOf(last), e.ViewModel);
                                    }
                                }));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-IViewModelCreated<IFooterViewModel>",
                        Processes.IntialSystemProcess, e => e != null, new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => e.ViewModel.Orientation == typeof(IFooterViewModel)
                        }, (s, e) =>
                        {
                            if (Application.Current == null)
                            {
                                s.FooterViewModels.Insert(e.ViewModel.Priority, e.ViewModel);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    var last = s.FooterViewModels.FirstOrDefault(x => x.Priority > e.ViewModel.Priority);
                                    if (last == null)
                                    {
                                        s.FooterViewModels.Add(e.ViewModel);
                                    }
                                    else
                                    {
                                        s.FooterViewModels.Insert(s.FooterViewModels.IndexOf(last), e.ViewModel);
                                    }
                                }));
                            }
                        }),
                    new ViewEventSubscription<IScreenModel, IViewModelCreated<IViewModel>>(
                        "ScreenViewModel-IViewModelCreated<IBodyViewModel>", Processes.IntialSystemProcess, e => e != null,
                        new List<Func<IScreenModel, IViewModelCreated<IViewModel>, bool>>
                        {
                            (s, e) => e.Process.Id != s.Process.Id &&
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

                    

                    new ViewEventSubscription<IScreenModel, ICleanUpSystemProcess>(
                        "ScreenViewModel-ICleanUpSystemProcess",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IScreenModel, ICleanUpSystemProcess, bool>> { },
                        (s, e) =>
                        {

                            if (Application.Current == null)
                            {
                                ClearScreenModels(s);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    ClearScreenModels(s);
                                }));
                            }
                        }),

                    new ViewEventSubscription<IScreenModel, ICurrentApplicationChanged>(
                        "ScreenViewModel-ICleanUpSystemProcess",
                        Processes.IntialSystemProcess,
                        e => e != null,
                        new List<Func<IScreenModel, ICurrentApplicationChanged, bool>> { },
                        (s, e) =>
                        {

                            if (Application.Current == null)
                            {
                                ClearScreenModels(s);
                            }
                            else
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    ClearScreenModels(s);
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
                            new StateEventInfo(s.Process, Context.ViewModel.Events.ViewModelLoaded),
                            s.BodyViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelLeft",
                        subject: v => v.LeftViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.LeftViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.LeftViewModels.Last()},
                            new StateEventInfo(s.Process, Context.ViewModel.Events.ViewModelLoaded),
                            s.LeftViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelHeader",
                        subject: v => v.HeaderViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.HeaderViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.HeaderViewModels.Last()},
                            new StateEventInfo(s.Process, Context.ViewModel.Events.ViewModelLoaded),
                            s.HeaderViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelRight",
                        subject: v => v.RightViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.RightViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.RightViewModels.Last()},
                            new StateEventInfo(s.Process, Context.ViewModel.Events.ViewModelLoaded),
                            s.RightViewModels.Last().Process, s.Source)),

                    new ViewEventPublication<IScreenModel, IViewModelLoaded<IScreenModel, IViewModel>>(
                        key: "ScreenModelFooter",
                        subject: v => v.FooterViewModels.CollectionChanges,
                        subjectPredicate: new List<Func<IScreenModel, bool>>
                        {
                            v => v.FooterViewModels.LastOrDefault() != null
                        },
                        messageData: s => new ViewEventPublicationParameter(new object[] {s, s.FooterViewModels.Last()},
                            new StateEventInfo(s.Process, Context.ViewModel.Events.ViewModelLoaded),
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
                ),
                new List<IViewModelInfo>());
        }

        private void ClearScreenModels(IScreenModel s)//, ICleanUpSystemProcess e
        {
            //foreach (var vm in s.BodyViewModels.Where(x => x.Process.Id == e.ProcessToBeCleanedUp.Id).ToList())
            //{
            //    s.BodyViewModels.Remove(vm);
            //}
            s.BodyViewModels.Clear();
           s.BodyViewModels.Reset();

        }
    }
}