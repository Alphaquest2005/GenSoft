using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using BootStrapper;
using Common.Dynamic;
using Interfaces;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;

namespace RevolutionData
{
    
    public class SummaryListViewModelInfo<TView> where TView : IEntityView
    {
        public static ViewModelInfo SummaryListViewModel(int processId, string symbol, string description, int priority, List<EntityViewModelRelationship> parentEntities)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new ViewInfo($"{typeof(TView).Name}SummaryListViewModel", symbol, description),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        new ViewEventSubscription<ISummaryListViewModel<TView>, IUpdateProcessStateList<TView>>(
                            3,
                            e => e != null,
                            new List<Func<ISummaryListViewModel<TView>, IUpdateProcessStateList<TView>, bool>>(),
                            (v,e) =>
                            {
                                if (v.State.Value == e.State) return;
                                v.State.Value = e.State;
                            }),


                        new ViewEventSubscription<ISummaryListViewModel<TView>, IEntityViewSetWithChangesLoaded<TView>>(
                            processId: processId,
                            eventPredicate: e => e.Changes.Count == 0,
                            actionPredicate: new List<Func<ISummaryListViewModel<TView>, IEntityViewSetWithChangesLoaded<TView>, bool>>(),
                            action: (v, e) =>
                            {
                                if (Application.Current == null)
                                {
                                    ReloadEntitySet(v, e);
                                }
                                else
                                {
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() => ReloadEntitySet(v, e)));
                                }
                            }),

                        new ViewEventSubscription<ISummaryListViewModel<TView>, IEntityViewWithChangesUpdated<TView>>(
                            processId: processId,
                            eventPredicate: e => e.Changes.Count > 0,
                            actionPredicate: new List<Func<ISummaryListViewModel<TView>, IEntityViewWithChangesUpdated<TView>, bool>>(),
                            action: (v, e) =>
                            {
                                if (Application.Current == null)
                                {
                                    UpdateEntitySet(v, e);
                                }
                                else
                                {
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() => UpdateEntitySet(v, e)));
                                }
                            }),

                        new ViewEventSubscription<ISummaryListViewModel<TView>, ICurrentEntityChanged<TView>>(
                            3,
                            e => e != null,
                            new List<Func<ISummaryListViewModel<TView>, ICurrentEntityChanged<TView>, bool>>(),
                            (v, e) =>
                            {
                                if (v.CurrentEntity.Value?.Id == e.Entity?.Id) return;
                                v.CurrentEntity.Value = e.Entity;
                            }),


                    },
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<ISummaryListViewModel<TView>, IViewStateLoaded<ISummaryListViewModel<TView>,IProcessStateList<TView>>>(
                            key:"ViewStateLoaded",
                            subject:v => v.State,
                            subjectPredicate:new List<Func<ISummaryListViewModel<TView>, bool>>
                            {
                                v => v.State != null
                            },
                            messageData:s =>
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    s.EntitySet.Value.Add(BootStrapper.BootStrapper.Container.GetConcreteInstance<TView>(typeof(TView)));
                                    s.NotifyPropertyChanged(nameof(s.EntitySet));
                                }));

                                return new ViewEventPublicationParameter(new object[] {s, s.State.Value},
                                    new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process,
                                    s.Source);
                            }),

                        new ViewEventPublication<ISummaryListViewModel<TView>, ICurrentEntityChanged<TView>>(
                            key:"CurrentEntityChanged",
                            subject:v =>  (IObservable<dynamic>)v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                            subjectPredicate:new List<Func<ISummaryListViewModel<TView>, bool>>{},
                            messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value},new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded),s.Process,s.Source))
                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {


                        new ViewEventCommand<ISummaryListViewModel<TView>, ILoadEntityViewSetWithChanges<TView,IPartialMatch>>(
                            key:"Search",
                            commandPredicate:new List<Func<ISummaryListViewModel<TView>, bool>>
                            {
                                v => v.ChangeTracking.Values.Count > 0

                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                            messageData: s =>
                            {
                                //ToDo: bad practise
                                if (!string.IsNullOrEmpty(((dynamic)s).Field) && !string.IsNullOrEmpty(((dynamic) s).Value))
                                {
                                    s.ChangeTracking.AddOrUpdate(((dynamic) s).Field, ((dynamic) s).Value);
                                }

                                return new ViewEventCommandParameter(
                                    new object[] {s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)},
                                    new StateCommandInfo(s.Process.Id,
                                        Context.EntityView.Commands.LoadEntityViewSetWithChanges), s.Process,
                                    s.Source);
                            }),

                        new ViewEventCommand<ISummaryListViewModel<TView>, IViewRowStateChanged<TView>>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<ISummaryListViewModel<TView>, bool>>
                            {
                                v => v.CurrentEntity != null
                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                            messageData: s =>
                            {
                                s.RowState.Value = s.RowState.Value != RowState.Modified?RowState.Modified: RowState.Unchanged;//new ReactiveProperty<RowState>(rowstate != RowState.Modified?RowState.Modified: RowState.Unchanged);

                                return new ViewEventCommandParameter(
                                    new object[] {s,s.RowState.Value},
                                    new StateCommandInfo(s.Process.Id,
                                        Context.Process.Commands.CurrentEntityChanged), s.Process,
                                    s.Source);
                            }),



                    },
                    viewModelType: typeof(ISummaryListViewModel<TView>),
                    orientation: typeof(IBodyViewModel),
                    priority: priority);

                var parentSubscriptions = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
                var parentCommands = new List<IViewModelEventCommand<IViewModel, IEvent>>();
                foreach (var p in parentEntities)
                {
                    parentSubscriptions.AddRange(CreateParentEntitySubscibtion(processId, p.ParentType, p.ViewParentProperty));

                }
                viewInfo.Subscriptions.AddRange(parentSubscriptions);

                parentCommands.AddRange(CreateParentEntityCommands(parentEntities.Select(x => x.ChildProperty).ToList()));


                return viewInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateParentEntityCommands(List<string> childProperty)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
            res.Add(CreateEntityCommand(childProperty));
            res.Add(EditEntityCommand(childProperty));
            return res;
        }


        public static IViewModelEventCommand<IViewModel, IEvent> CreateEntityCommand(List<string> childProperties)
        {
            return  new ViewEventCommand<ISummaryListViewModel<TView>, IUpdateEntityViewWithChanges<TView>>(
                key: "EditEntity",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<ISummaryListViewModel<TView>, bool>>
                {
                    v => v.ChangeTracking.Count == 1 && v.CurrentEntity.Value.Id != 0

                },
                //TODO: Make a type to capture this info... i killing it here
                messageData: v =>
                {
                    foreach (var childProperty in childProperties)
                    {
                        v.ChangeTracking.Add(nameof(childProperty), ((dynamic)v).Properties["childProperty"]);
                    }
                    
                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            v.CurrentEntity.Value.Id,
                            v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
                        v.Source);
                    v.ChangeTracking.Clear();
                    return msg;
                });
        }

        public static IViewModelEventCommand<IViewModel, IEvent> EditEntityCommand(List<string> childProperties)
        {
            return new ViewEventCommand<ISummaryListViewModel<TView>, IUpdateEntityViewWithChanges<TView>>(
                key: "CreateEntity",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<ISummaryListViewModel<TView>, bool>>
                {
                    v => v.ChangeTracking.Count > 0 && v.CurrentEntity.Value.Id == 0

                },
                //TODO: Make a type to capture this info... i killing it here
                messageData: v =>
                {
                    foreach (var childProperty in childProperties)
                    {
                        v.ChangeTracking.Add(nameof(childProperty), ((dynamic)v).Properties["childProperty"]);
                    }
                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            v.CurrentEntity.Value.Id,
                            v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
                        v.Source);
                    v.ChangeTracking.Clear();
                    return msg;
                });
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentEntitySubscibtion(int processId, Type parentEntity, string parentProperty)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>) typeof(SummaryListViewModelInfo<TView>).GetMethod("ParentCurrentEntityChanged").MakeGenericMethod(parentEntity).Invoke(null, new object[] { processId, parentProperty}));
            return res;
        }





        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged<PEntity>(int processId,string parentProperty)
        {
            return new ViewEventSubscription<ISummaryListViewModel<TView>, ICurrentEntityChanged<PEntity>>(
                processId,
                e => e != null,
                new List<Func<ISummaryListViewModel<TView>, ICurrentEntityChanged<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((Expando)v).Properties[parentProperty] = e.Entity;
                });
        }

        private static void UpdateEntitySet(ISummaryListViewModel<TView> summaryListViewModel,
            IEntityViewWithChangesUpdated<TView> msg)
        {
            var existingEntity = summaryListViewModel.EntitySet.Value.FirstOrDefault(x => x.Id == msg.Entity.Id);
            if (existingEntity != null) summaryListViewModel.EntitySet.Value.Remove(existingEntity);

            summaryListViewModel.EntitySet.Value.Add(msg.Entity);
            summaryListViewModel.EntitySet.Value.Reset();

        }

        private static void ReloadEntitySet(ISummaryListViewModel<TView> v, IEntityViewSetWithChangesLoaded<TView> e)
        {
            v.EntitySet.Value.Clear();
            v.EntitySet.Value.AddRange(e.EntitySet);
            v.EntitySet.Value.Reset();
        }

       
    }


}