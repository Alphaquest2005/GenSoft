using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using DomainUtilities;
using GenSoft.Entities;
using JB.Collections.Reactive;
using Reactive.Bindings;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModel.WorkFlow;
using Action = System.Action;
using Application = System.Windows.Application;
using IEvent = SystemInterfaces.IEvent;
using IViewModel = ViewModel.Interfaces.IViewModel;

namespace RevolutionData
{
    
    public class SummaryListViewModelInfo
    {
        public static ViewModelInfo SummaryListViewModel(int processId, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality, string symbol, string description, int priority, List<EntityViewModelRelationship> viewRelationships, List<EntityTypeViewModelCommand> viewCommands, IViewAttributeDisplayProperties displayProperties)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new EntityViewInfo($"{entityType.Name}-SummaryListViewModel", symbol, description,entityType, ordinality),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        //new ViewEventSubscription<ISummaryListViewModel, IViewModelIntialized>(
                        //    $"{entityType.Name}-IViewModelIntialized",
                        //    processId,
                        //    e => e.ViewModel != null,
                        //    new List<Func<ISummaryListViewModel, IViewModelIntialized, bool>>(),
                        //    (v,e) =>
                        //    {
                        //        if (e.ViewModel.ViewInfo.Name != v.ViewInfo.Name) return;
                        //        v.EntitySet.Value.Add(new DynamicEntity(entityType,0,new Dictionary<string, object>(){{ "EntityName", "Create New..." } }));
                        //        v.EntitySet.Value.Reset();
                        //    }),

                        new ViewEventSubscription<ISummaryListViewModel, IUpdateProcessStateList>(
                            $"{entityType.Name}-IUpdateProcessStateList",
                            processId,
                            e => e.EntityType == entityType,
                            new List<Func<ISummaryListViewModel, IUpdateProcessStateList, bool>>(),
                            (v,e) =>
                            {
                                if (e.State != null && v.State.Value != null && v.State.Value.EntitySet.SequenceEqual(e.State.EntitySet)) return;
                                v.State.Value = e.State;
                                v.CurrentEntity.Value = v.EntitySet.Value.First();

                            }),


                        new ViewEventSubscription<ISummaryListViewModel, IEntityWithChangesUpdated>(
                            key:$"{entityType.Name}-IEntityWithChangesUpdated",
                            processId: processId,
                            eventPredicate: e => e.Changes.Count > 0 && e.EntityType == entityType,
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
                            $"{entityType.Name}-ICurrentEntityChanged",
                            processId,
                            e => e.EntityType == entityType,
                            new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                            (v, e) =>
                            {
                                if (Equals(v.CurrentEntity.Value, e.Entity)) return;
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
                                   //s.NotifyPropertyChanged(nameof(s.EntitySet));
                                }));

                                return new ViewEventPublicationParameter(new object[] {s, s.State.Value},
                                    new RevolutionEntities.Process.StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process,
                                    s.Source);
                            }),

                        new ViewEventPublication<ISummaryListViewModel, ICurrentEntityChanged>(
                            key:"CurrentEntityChanged",
                            subject:v =>  v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>{},
                            messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value ?? DynamicEntity.NullEntity},new RevolutionEntities.Process.StateEventInfo(s.Process.Id, Context.View.Events.CurrentEntityChanged),s.Process,s.Source)),

                        new ViewEventPublication<ISummaryListViewModel, IViewModelIntialized>(
                            key:$"{entityType.Name}-IViewModelIntialized",
                            subject:v => v.ViewModelState,
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Intialized},
                            messageData:v => new ViewEventPublicationParameter(new object[] {v},new RevolutionEntities.Process.StateEventInfo(v.Process.Id, Context.View.Events.Intitalized),v.Process,v.Source)),

                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {
                        new ViewEventCommand<ISummaryListViewModel, IMainEntityChanged>(
                            key:"ChangeMainEntity",
                            commandPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                              //  v => v.CurrentEntity.Value != null
                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                            messageData: s =>
                            {
                               
                                return new ViewEventCommandParameter(
                                    new object[] {s.CurrentEntity.Value ?? s.ViewInfo.EntityType.DefaultEntity()},
                                    new RevolutionEntities.Process.StateCommandInfo(s.Process.Id,
                                        Context.Process.Commands.CurrentEntityChanged), s.Process,
                                    s.Source);
                            }),

                        new ViewEventCommand<ISummaryListViewModel, IViewModelVisibilityChanged>(
                            key:"ChangeViewModelVisibility",
                            commandPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                                //  v => v.CurrentEntity.Value != null
                            },
                            subject:v => Observable.Empty<ReactiveCommand<IViewModel>>(),

                            messageData: v =>
                            {
                                v.SelectedViewModel.Value.Visibility.Value =  v.SelectedViewModel.Value.Visibility.Value == Visibility.Visible
                                    ? Visibility.Collapsed
                                    : Visibility.Visible;

                                return new ViewEventCommandParameter(
                                    new object[] {v.SelectedViewModel.Value, v.SelectedViewModel.Value.Visibility.Value},
                                    new RevolutionEntities.Process.StateCommandInfo(v.Process.Id,
                                        Context.ViewModel.Commands.ChangeVisibility), v.Process,
                                    v.Source);
                            }),

                        //Todo: supposed to be create from database
                        new ViewEventCommand<ISummaryListViewModel, IViewRowStateChanged>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                              //  v => v.CurrentEntity.Value != null
                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                            messageData: s =>
                            {
                                s.RowState.Value = s.RowState.Value != RowState.Modified?RowState.Modified: RowState.Loaded;


                                return new ViewEventCommandParameter(
                                    new object[] {s,s.RowState.Value},
                                    new RevolutionEntities.Process.StateCommandInfo(s.Process.Id,
                                        Context.Process.Commands.CurrentEntityChanged), s.Process,
                                    s.Source);
                            }),




                    },
                    viewModelType: typeof(ISummaryListViewModel),
                    orientation: typeof(IBodyViewModel),
                    priority: priority,
                    displayProperties: displayProperties);

                var parentSubscriptions = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
                var parentCommands = new List<IViewModelEventCommand<IViewModel, IEvent>>();
                foreach (var p in viewRelationships.Select(x => x.ParentType).Distinct())
                {
                    parentSubscriptions.AddRange(CreateParentEntitySubscibtion(processId, p, p));

                }
                viewInfo.Subscriptions.AddRange(parentSubscriptions);

                parentCommands.AddRange(CreateParentEntityCommands(viewRelationships.Select(x => x.ChildProperty).Distinct().ToList()));
                parentCommands.AddRange(CreateCustomCommands(viewCommands, viewRelationships));

                viewInfo.Commands.AddRange(parentCommands);

                return viewInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }



        

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentEntitySubscibtion(int processId, string parentEntity, string parentProperty)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>) typeof(SummaryListViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId,DynamicEntityType.DynamicEntityTypes[parentEntity], parentProperty}));
            return res;
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(int processId, IDynamicEntityType pEntity,string parentProperty)
        {
            return new ViewEventSubscription<ISummaryListViewModel, ICurrentEntityChanged>(
                $"{pEntity.Name}-ICurrentEntityChanged",
                processId,
                e => e != null && e.Entity?.EntityType == pEntity,
                new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    v.ParentEntities.AddOrUpdate(e.Entity);
                });
        }
        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateParentEntityCommands(List<string> childProperty)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
            
            return res;
        }

        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateCustomCommands(List<EntityTypeViewModelCommand> viewCommands, List<EntityViewModelRelationship> parentEntites)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();

            
            foreach (var cmd in viewCommands)
            {
                 res.Add(ViewModelInfoExtensions.CreateCustomCommand<ISummaryListViewModel>(cmd.ViewModelCommands, parentEntites));
            }
            return res;
        }




        private static void UpdateEntitySet(ISummaryListViewModel summaryListViewModel, IEntityWithChangesUpdated msg)
        {
            var res = summaryListViewModel.EntitySet.Value.ToList();
            var existingEntity = res.FirstOrDefault(x => x.Id == msg.Entity.Id);
            IDynamicEntity newEntity;
            if (existingEntity == null)
            {
                newEntity = new DynamicEntity(msg.EntityType, msg.Entity.Id, msg.Changes);
            }
            else
            {
                newEntity = existingEntity.ApplyChanges(msg.Changes);
                res.Remove(existingEntity);
            }


            res.Insert(0, newEntity);
            summaryListViewModel.EntitySet.Value = new ObservableList<IDynamicEntity>(res);
            summaryListViewModel.EntitySet.Value.Reset();

            summaryListViewModel.RowState.Value = RowState.Unchanged;


        }

        private static void ReloadEntitySet(ISummaryListViewModel v, IList<IDynamicEntity> e, IDynamicEntityType entityType)
        {
            v.EntitySet.Value.Clear();
            v.EntitySet.Value.AddRange(e);
            v.EntitySet.Value.Add(entityType.DefaultEntity());
            v.EntitySet.Value.Reset();
        }

       
    }


}

