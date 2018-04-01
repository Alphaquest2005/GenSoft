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
using RevolutionEntities.Process;
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
        public static ViewModelInfo SummaryListViewModel(ISystemProcess process, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality, string symbol, string description, int priority,List<IViewModelInfo> childViews, List<EntityViewModelRelationship> viewRelationships, List<EntityTypeViewModelCommand> viewCommands, IViewAttributeDisplayProperties displayProperties)
        {
            try
            {
                var sublst = new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    
                    new ViewEventSubscription<ISummaryListViewModel, IUpdateProcessStateList>(
                        $"{entityType.Name}-IUpdateProcessStateList",
                        process,
                        e => e.EntityType.Name == entityType.Name && e.Process.Id == process.Id,
                        new List<Func<ISummaryListViewModel, IUpdateProcessStateList, bool>>(),
                        (v, e) =>
                        {
                            if (v.ViewModelState == null) return;
                            if (e.State != null && v.State.Value != null &&
                                v.State.Value.EntitySet.Select(x => x.Id).SequenceEqual(e.State.EntitySet.Select(x => x.Id))) return;
                            v.ViewModelState.Value = ViewModelState.LoadingData;
                            v.State.Value = e.State;
                            if (v.EntitySet.Value.Any() && v.CurrentEntity.Value?.Id  != v.EntitySet.Value.First().Id) v.CurrentEntity.Value = v.EntitySet.Value.First();
                            v.ViewModelState.Value = ViewModelState.StopLoadingData;
                            

                        }, new RevolutionEntities.Process.StateCommandInfo(process, RevolutionData.Context.CommandFunctions.UpdateCommandData(entityType.Name, Context.Entity.Commands.UpdateState), Guid.NewGuid())),


                    new ViewEventSubscription<ISummaryListViewModel, IEntityWithChangesUpdated>(
                        key: $"{entityType.Name}-IEntityWithChangesUpdated",
                        process: process,
                        eventPredicate: e => e.Changes.Count > 0 && e.EntityType.Name == entityType.Name && e.Process.Id == process.Id,
                        actionPredicate: new List<Func<ISummaryListViewModel, IEntityWithChangesUpdated, bool>>(),
                        action: (v, e) =>
                        {
                            UpdateEntitySet(v, e);
                        }, processInfo: new RevolutionEntities.Process.StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, Context.Entity.Events.EntityUpdated), Guid.NewGuid())),

                    new ViewEventSubscription<ISummaryListViewModel, ICurrentEntityChanged>(
                        $"{entityType.Name}-ICurrentEntityChanged",
                        process,
                        e => e.EntityType.Name == entityType.Name &&  e.Entity.Id > 0 && e.Process.Id == process.Id,
                        new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                        (v, e) =>
                        {
                            //if (e.Source.SourceName == v.Source.SourceName) return;
                            if (Equals(v.CurrentEntity.Value?.Id, e.Entity?.Id)) return;
                            v.CurrentEntity.Value = e.Entity;
                            
                        }, new RevolutionEntities.Process.StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, Context.ViewModel.Events.CurrentEntityChanged), Guid.NewGuid())),

                    new ViewEventSubscription<ISummaryListViewModel, IEntityDeleted>(
                        key: $"{entityType.Name}-IEntityDeleted",
                        process: process,
                        eventPredicate: e => e.EntityType.Name == entityType.Name && e.Process.Id == process.Id,
                        actionPredicate: new List<Func<ISummaryListViewModel, IEntityDeleted, bool>>(),
                        action: (v, e) =>
                        {
                           RemoveItemFromEntitySet(v, e);
                        }, processInfo: new RevolutionEntities.Process.StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, Context.Entity.Events.EntityUpdated), Guid.NewGuid())),


                };

               var cmdLst = new List<IViewModelEventCommand<IViewModel, IEvent>>
                {
                    new ViewEventCommand<ISummaryListViewModel, IMainEntityChanged>(
                        key: $"ChangeMainEntity",
                        commandPredicate: new List<Func<ISummaryListViewModel, bool>>
                        {
                            //  v => v.CurrentEntity.Value != null
                        },
                        subject: s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                        messageData: s =>
                        {

                            return new ViewEventCommandParameter(
                                new object[] {s.CurrentEntity.Value ?? s.ViewInfo.EntityType.DefaultEntity()},
                                new RevolutionEntities.Process.StateCommandInfo(s.Process,
                                    Context.Process.Commands.ChangeMainEntity), s.Process,
                                s.Source);
                        }),

                    new ViewEventCommand<ISummaryListViewModel, IViewModelVisibilityChanged>(
                        key: $"ChangeViewModelVisibility",
                        commandPredicate: new List<Func<ISummaryListViewModel, bool>>
                        {
                              v => v.SelectedViewModel.Value != null
                        },
                        subject: v => Observable.Empty<ReactiveCommand<IViewModel>>(),

                        messageData: v =>
                        {
                            
                            v.SelectedViewModel.Value.Visibility.Value =
                                v.SelectedViewModel.Value.Visibility.Value == Visibility.Visible
                                    ? Visibility.Collapsed
                                    : Visibility.Visible;

                            return new ViewEventCommandParameter(
                                new object[] {v.SelectedViewModel.Value, v.SelectedViewModel.Value.Visibility.Value},
                                new RevolutionEntities.Process.StateCommandInfo(v.Process,
                                    Context.View.Commands.ChangeVisibility), v.Process,
                                v.Source);
                        }),

                    //Todo: supposed to be create from database
                    new ViewEventCommand<ISummaryListViewModel, IViewRowStateChanged>(
                        key: $"EditEntity",
                        commandPredicate: new List<Func<ISummaryListViewModel, bool>>
                        {
                            //  v => v.CurrentEntity.Value != null
                        },
                        subject: s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                        messageData: s =>
                        {
                            s.RowState.Value = s.RowState.Value != RowState.Modified
                                ? RowState.Modified
                                : RowState.Loaded;
                            

                            return new ViewEventCommandParameter(
                                new object[] {s, s.RowState.Value},
                                new RevolutionEntities.Process.StateCommandInfo(s.Process,
                                    Context.ViewModel.Commands.ChangeCurrentEntity), s.Process,
                                s.Source);
                        }),

                    new ViewEventCommand<ISummaryListViewModel, IStartAddin>(
                        key: $"AddinAction",
                        commandPredicate: new List<Func<ISummaryListViewModel, bool>>
                        {
                              v => v.SelectedAddinAction.Value != null
                        },
                        subject: s => s.SelectedAddinAction,

                        messageData: s =>
                        {
                            var viewEventCommandParameter = new ViewEventCommandParameter(
                                new object[] {s.SelectedAddinAction.Value, s.CurrentEntity.Value},
                                new RevolutionEntities.Process.StateCommandInfo(s.Process,
                                    Context.CommandFunctions.UpdateCommandData(s.SelectedAddinAction.Value.Name,Context.Addin.Commands.StartAddin)), s.Process,
                                s.Source);
                            s.SelectedAddinAction.Value = null;
                            return viewEventCommandParameter;
                        }),

                    new ViewEventCommand<ISummaryListViewModel, IDeleteEntity>(
                        key: $"DeleteEntity",
                        commandPredicate: new List<Func<ISummaryListViewModel, bool>>
                        {
                              //v => v.CurrentEntity.Value != null
                        },
                        subject: s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                        messageData: s =>
                        {
                            return new ViewEventCommandParameter(
                                new object[] {s.CurrentEntity.Value},
                                new RevolutionEntities.Process.StateCommandInfo(s.Process,
                                    Context.CommandFunctions.UpdateCommandData($@"{s.CurrentEntity.Value.EntityType.Name}-{s.CurrentEntity.Value.Id}", Context.Entity.Commands.DeleteEntity)), s.Process,
                                s.Source);
                        }),
                };
                var parentSubscriptions = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
                var parentCommands = new List<IViewModelEventCommand<IViewModel, IEvent>>();
                foreach (var p in viewRelationships.Select(x => x.ParentType).Distinct())
                {
                    parentSubscriptions.AddRange(CreateParentEntitySubscibtion(process, p, p));
                }
                sublst.AddRange(parentSubscriptions);

                parentCommands.AddRange(CreateParentEntityCommands(viewRelationships.Select(x => x.ChildProperty).Distinct().ToList()));
                parentCommands.AddRange(CreateCustomCommands(viewCommands, viewRelationships));

                cmdLst.AddRange(parentCommands);

                var viewInfo = new ViewModelInfo
                (
                    process: process,
                    viewInfo: new EntityViewInfo($"{entityType.Name}-SummaryListViewModel", symbol, description,entityType, ordinality),
                    subscriptions: sublst ,
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<ISummaryListViewModel, IViewStateLoaded<ISummaryListViewModel,IProcessStateList>>(
                            key:$"{entityType.Name}-ViewStateLoaded",
                            subject:v => v.State,
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                                v => v.State != null
                            },
                            messageData:s =>
                            {

                                return new ViewEventPublicationParameter(new object[] {s, s.State.Value},
                                    new RevolutionEntities.Process.StateEventInfo(s.Process, Context.EventFunctions.UpdateEventData(s.ViewInfo.EntityType.Name, Context.View.Events.ProcessStateLoaded)), s.Process,
                                    s.Source);
                            }),

                        new ViewEventPublication<ISummaryListViewModel, ICurrentEntityChanged>(
                            key:$"{entityType.Name}-CurrentEntityChanged",
                            subject:v =>  v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>
                            {
                                v => v.ViewModelState != null && v.ViewModelState?.Value != ViewModelState.LoadingData
                            },
                            messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value ?? DynamicEntity.NullEntity},new RevolutionEntities.Process.StateEventInfo(s.Process, Context.EventFunctions.UpdateEventData(s.ViewInfo.EntityType.Name,Context.ViewModel.Events.CurrentEntityChanged)),s.Process,s.Source)),

                        new ViewEventPublication<ISummaryListViewModel, IViewModelInitialized>(
                            key:$"{entityType.Name}-IViewModelInitialized",
                            subject:v => v.ViewModelState,
                            subjectPredicate:new List<Func<ISummaryListViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Initialized},
                            messageData:v => new ViewEventPublicationParameter(new object[] {v},new RevolutionEntities.Process.StateEventInfo(v.Process, Context.EventFunctions.UpdateEventData(v.ViewInfo.EntityType.Name,Context.ViewModel.Events.Initialized)),v.Process,v.Source)),

                    },
                    commands: cmdLst ,
                    viewModelInfos: childViews,
                    viewModelType: typeof(ISummaryListViewModel),
                    orientation: typeof(IBodyViewModel),
                    priority: priority,
                    displayProperties: displayProperties);



                return viewInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static void RemoveItemFromEntitySet(ISummaryListViewModel summaryListViewModel, IEntityDeleted msg)
        {
            var res = summaryListViewModel.EntitySet.Value.ToList();
            var existingEntity = res.FirstOrDefault(x => x.Id == msg.Entity.Id);

            res.Remove(existingEntity);
            summaryListViewModel.EntitySet.Value = new ObservableList<IDynamicEntity>(res);
            summaryListViewModel.EntitySet.Value.Reset();
            summaryListViewModel.RowState.Value = RowState.Unchanged;
        }


        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentEntitySubscibtion(ISystemProcess processId, string parentEntity, string parentProperty)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>) typeof(SummaryListViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId,DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentEntity), parentProperty}));
            return res;
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(ISystemProcess process, IDynamicEntityType pEntity,string parentProperty)
        {
            return new ViewEventSubscription<ISummaryListViewModel, ICurrentEntityChanged>(
                $"{pEntity.Name}-ICurrentEntityChanged",
                process,
                e => e != null && e.Process.Id == process.Id && e.Entity?.EntityType == pEntity,
                new List<Func<ISummaryListViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    v.ParentEntities.AddOrUpdate(e.Entity);
                }, new RevolutionEntities.Process.StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(pEntity.Name, Context.ViewModel.Events.CurrentEntityChanged), Guid.NewGuid()));
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
                 res.Add(ViewModelInfoExtensions.CreateCustomCommand<ISummaryListViewModel>(cmd.ViewModelCommand, parentEntites));
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

        //private static void ReloadEntitySet(ISummaryListViewModel v, IList<IDynamicEntity> e, IDynamicEntityType entityType)
        //{
        //    v.EntitySet.Value.Clear();
        //    v.EntitySet.Value.AddRange(e);
        //    v.EntitySet.Value.Add(entityType.DefaultEntity());
        //    v.EntitySet.Value.Reset();
        //}

       
    }


}

