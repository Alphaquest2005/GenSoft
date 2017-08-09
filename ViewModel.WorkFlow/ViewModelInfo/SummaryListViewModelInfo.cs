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
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;

namespace RevolutionData
{
    
    public class SummaryListViewModelInfo
    {
        public static ViewModelInfo SummaryListViewModel(int processId,string entityType, string symbol, string description, int priority, List<EntityViewModelRelationship> parentEntities)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new ViewInfo($"{entityType}SummaryListViewModel", symbol, description),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        new ViewEventSubscription<ISummaryListViewModel, IUpdateProcessStateList>(
                            3,
                            e => e != null,
                            new List<Func<ISummaryListViewModel, IUpdateProcessStateList, bool>>(),
                            (v,e) =>
                            {
                                if (v.State.Value == e.State) return;
                                v.State.Value = e.State;
                            }),


                        new ViewEventSubscription<ISummaryListViewModel, IEntitySetWithChangesLoaded>(
                            processId: processId,
                            eventPredicate: e => e.Changes.Count == 0,
                            actionPredicate: new List<Func<ISummaryListViewModel, IEntitySetWithChangesLoaded, bool>>(),
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

                        new ViewEventSubscription<ISummaryListViewModel, IEntityWithChangesUpdated>(
                            processId: processId,
                            eventPredicate: e => e.Changes.Count > 0,
                            actionPredicate: new List<Func<ISummaryListViewModel, IEntityWithChangesUpdated, bool>>(),
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

                        new ViewEventSubscription<ISummaryListViewModel, ICurrentEntityChanged>(
                            3,
                            e => e != null,
                            new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                            (v, e) =>
                            {
                                if (v.CurrentEntity.Value?.Id == e.Entity?.Id) return;
                                v.CurrentEntity.Value = e.Entity;
                            }),


                    },
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<ISummaryListViewModel, IViewStateLoaded<ISummaryListViewModel,IProcessStateList>>(
                            key:"ViewStateLoaded",
                            subject:v => v.State,
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                                v => v.State != null
                            },
                            messageData:s =>
                            {
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    //s.EntitySet.Value.Add(BootStrapper.BootStrapper.Container.GetConcreteInstance(typeof(TView)));
                                    s.NotifyPropertyChanged(nameof(s.EntitySet));
                                }));

                                return new ViewEventPublicationParameter(new object[] {s, s.State.Value},
                                    new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process,
                                    s.Source);
                            }),

                        new ViewEventPublication<ISummaryListViewModel, ICurrentEntityChanged>(
                            key:"CurrentEntityChanged",
                            subject:v =>  (IObservable<dynamic>)v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>{},
                            messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value},new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded),s.Process,s.Source))
                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {


                        new ViewEventCommand<ISummaryListViewModel, ILoadEntitySetWithChanges>(
                            key:"Search",
                            commandPredicate:new List<Func<ISummaryListViewModel, bool>>
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

                        new ViewEventCommand<ISummaryListViewModel, IViewRowStateChanged>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<ISummaryListViewModel, bool>>
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
                    viewModelType: typeof(ISummaryListViewModel),
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
            return  new ViewEventCommand<ISummaryListViewModel, IUpdateEntityWithChanges>(
                key: "EditEntity",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<ISummaryListViewModel, bool>>
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
            return new ViewEventCommand<ISummaryListViewModel, IUpdateEntityWithChanges>(
                key: "CreateEntity",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<ISummaryListViewModel, bool>>
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

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentEntitySubscibtion(int processId, string parentEntity, string parentProperty)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>) typeof(SummaryListViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId,parentEntity, parentProperty}));
            return res;
        }





        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(int processId, string pEntity,string parentProperty)
        {
            return new ViewEventSubscription<ISummaryListViewModel, ICurrentEntityChanged>(
                processId,
                e => e != null && e.Entity.EntityType == pEntity,
                new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    ((Expando)v).Properties[parentProperty] = e.Entity;
                });
        }

        private static void UpdateEntitySet(ISummaryListViewModel summaryListViewModel,IEntityWithChangesUpdated msg)
        {
            var existingEntity = summaryListViewModel.EntitySet.Value.FirstOrDefault(x => x.Id == msg.Entity.Id);
            if (existingEntity != null) summaryListViewModel.EntitySet.Value.Remove(existingEntity);

            summaryListViewModel.EntitySet.Value.Add(msg.Entity);
            summaryListViewModel.EntitySet.Value.Reset();

        }

        private static void ReloadEntitySet(ISummaryListViewModel v, IEntitySetWithChangesLoaded e)
        {
            v.EntitySet.Value.Clear();
            v.EntitySet.Value.AddRange(e.EntitySet);
            v.EntitySet.Value.Reset();
        }

       
    }


}