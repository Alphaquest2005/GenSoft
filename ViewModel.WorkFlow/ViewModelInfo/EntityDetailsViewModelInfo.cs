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
using DomainMessages;
using Interfaces;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace RevolutionData
{
    
    public class EntityDetailsViewModelInfo<TView> where TView : IEntityId
    {
        public static ViewModelInfo EntityDetailsViewModel(int processId, string symbol, string description, int priority, List<ViewModelEntity> parentEntities, List<ViewModelEntity> childEntities)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new ViewInfo("EntityDetailsViewModel", symbol, description),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>{},
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>{},
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>{},
                    viewModelType: typeof(IEntityDetailsViewModel<TView>),
                    orientation: typeof(IBodyViewModel),
                    priority: priority);

                var parentSubscriptions = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
                var parentCommands = new List<IViewModelEventCommand<IViewModel, IEvent>>();
                var parentPublications = new List<IViewModelEventPublication<IViewModel, IEvent>>();
                foreach (var p in childEntities)
                {
                    parentSubscriptions.AddRange(CreateChildSubscibtions(processId,p));
                    //parentPublications.AddRange(CreatePublications(p));
                    //parentCommands.AddRange(CreateCommands(p));
                }

                foreach (var p in parentEntities)
                {
                    parentSubscriptions.AddRange(CreateParentSubscibtions(processId, p));
                    //parentPublications.AddRange(CreatePublications(p));
                    //parentCommands.AddRange(CreateCommands(p));
                }
                viewInfo.Subscriptions.AddRange(parentSubscriptions);

                


                return viewInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentSubscibtions(int processId, ViewModelEntity relationship)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo<TView>).GetMethod("ParentCurrentEntityChanged").MakeGenericMethod(relationship.EntityType).Invoke(null, new object[] { processId, relationship.ViewProperty }));
            return res;
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateChildSubscibtions(int processId, ViewModelEntity relationship)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo<TView>).GetMethod("RecieveProcessStateMessage").MakeGenericMethod(relationship.EntityType).Invoke(null, new object[] { processId, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo<TView>).GetMethod("EntityFound").MakeGenericMethod(relationship.EntityType).Invoke(null, new object[] { processId, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo<TView>).GetMethod("EntityViewWithChangesFound").MakeGenericMethod(relationship.EntityType).Invoke(null, new object[] { processId, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo<TView>).GetMethod("EntityViewSetWithChangesLoaded").MakeGenericMethod(relationship.EntityType).Invoke(null, new object[] { processId, relationship.ViewProperty }));
            return res;
        }
        private static List<IViewModelEventPublication<IViewModel, IEvent>> CreatePublications(EntityViewModelRelationship rel)
        {
            List<IViewModelEventPublication<IViewModel, IEvent>> res = new List<IViewModelEventPublication<IViewModel, IEvent>>();
            //res.Add((IViewModelEventPublication<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("RecieveProcessStateMessage").MakeGenericMethod(rel.ChildType).Invoke(null, new object[] { rel.ChildProperty }));

            return res;
        }

        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateCommands(EntityViewModelRelationship rel)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
            //res.Add((IViewModelEventCommand<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("RecieveProcessStateMessage").MakeGenericMethod(rel.ChildType).Invoke(null, new object[] {rel.ChildProperty }));
           
            return res;
        }


        public static IViewModelEventCommand<IViewModel, IEvent> SaveEntityCommand(string viewChildProperty,List<EntityViewModelRelationship> rels)
        {
            return new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdatePullEntityWithChanges>(
                key: "SaveEntity",
                subject: s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),
                commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
                {
                    v => v.ChangeTracking.Count >= 1 && ((dynamic)v).Properties[viewChildProperty].Id != 0
                },
                //TODO: Make a type to capture this info... i killing it here
                messageData: s =>
                {
                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            ((dynamic) s).Id,
                            "Patient",
                            "General",
                            "Personal Information",
                            s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(s.Process.Id, Context.EntityView.Commands.GetEntityView), s.Process,
                        s.Source);
                    s.ChangeTracking.Clear();
                    ((dynamic)s).Properties[viewChildProperty] = null;

                    return msg;
                });
        }

        public static IViewModelEventCommand<IViewModel, IEvent> EditEntityCommand(string viewChildProperty, List<EntityViewModelRelationship> rels)
        {
            return new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdatePullEntityWithChanges>(
                key: $"EditEntity{typeof(TView).Name}",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
                {
                    v => v.ChangeTracking.Count == 1 && ((dynamic)v).Properties[viewChildProperty].Id != 0

                },
                //TODO: Make a type to capture this info... i killing it here
                messageData: v =>
                {
                    foreach (var c in rels)
                    {
                        v.ChangeTracking.Add(nameof(c.ChildProperty), ((dynamic)v).Properties[c.ChildProperty]);
                    }

                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            ((dynamic)v).Properties[viewChildProperty].Id,
                            v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
                        v.Source);
                    v.ChangeTracking.Clear();
                    return msg;
                });
        }

        public static IViewModelEventCommand<IViewModel, IEvent> CreateEntityCommand(string childProperty, List<EntityViewModelRelationship> rels) 
        {
            return new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdatePullEntityWithChanges> (
                key: "CreateEntity",
                subject: v => v.ChangeTracking.DictionaryChanges,
                commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
                {
                    v => v.ChangeTracking.Count > 0 && ((dynamic)v).Properties[childProperty].Id == 0

                },
                //TODO: Make a type to capture this info... i killing it here
                messageData: v =>
                {
                    foreach (var c in rels)
                    {
                        v.ChangeTracking.Add(nameof(c.ChildProperty), ((dynamic)v).Properties[c.ChildProperty]);
                    }
                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            ((dynamic)v).Properties[childProperty].Id,
                            v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
                        v.Source);
                    v.ChangeTracking.Clear();
                    return msg;
                });
        }







        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged<PEntity>(int processId,string parentProperty)
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, ICurrentEntityChanged<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, ICurrentEntityChanged<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[parentProperty] = e.Entity;
                    v.NotifyPropertyChanged(parentProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> RecieveProcessStateMessage<PEntity>(int processId, string viewChildProperty) where PEntity : IEntityId
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, IProcessStateMessage<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, IProcessStateMessage<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.State.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityFound<PEntity>(int processId, string viewChildProperty) where PEntity : IEntityId
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, IEntityFound<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, IEntityFound<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityViewWithChangesFound<PEntity>(int processId, string viewChildProperty) where PEntity : IEntityId
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, IEntityViewWithChangesFound<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, IEntityViewWithChangesFound<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }


        public static IViewModelEventSubscription<IViewModel, IEvent> EntityViewSetWithChangesLoaded<PEntity>(int processId, string viewChildProperty) where PEntity : IEntityView
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, IEntityViewSetWithChangesLoaded<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, IEntityViewSetWithChangesLoaded<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.EntitySet;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        //public static IViewModelEventPublication<IViewModel, IEvent> IViewStateLoaded<TView>(int processId, string childProperty) where TView : IEntityId
        //{
        //    return new ViewEventPublication<IEntityDetailsViewModel, IViewStateLoaded<IEntityDetailsViewModel, IProcessState<TView>>>(
        //        key: "ViewStateLoaded",
        //        subject: v => v.WhenAny(),
        //        subjectPredicate: new List<Func<IEntityDetailsViewModel, bool>>
        //        {
        //            v => v.State != null
        //        },
        //        messageData: s => new ViewEventPublicationParameter(new object[] { s, s.State.Value }, new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process, s.Source)),;
        //}



    }

}