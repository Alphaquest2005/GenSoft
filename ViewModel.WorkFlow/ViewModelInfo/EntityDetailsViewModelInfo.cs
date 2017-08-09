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
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace RevolutionData
{
    
    public class EntityDetailsViewModelInfo
    {
        public EntityDetailsViewModelInfo(string entityType)
        {
            EntityType = entityType;
        }
        public static ViewModelInfo EntityDetailsViewModel(int processId, string entityType,  string symbol, string description, int priority, List<ViewModelEntity> parentEntities, List<ViewModelEntity> childEntities)
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
                    viewModelType: typeof(IEntityDetailsViewModel),
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
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId, relationship.EntityType, relationship.ViewProperty }));
            return res;
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateChildSubscibtions(int processId, ViewModelEntity relationship)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("RecieveProcessStateMessage").Invoke(null, new object[] { processId,relationship.EntityType, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityFound").Invoke(null, new object[] { processId, relationship.EntityType, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityViewWithChangesFound").Invoke(null, new object[] { processId, relationship.EntityType, relationship.ViewProperty }));
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityViewSetWithChangesLoaded").Invoke(null, new object[] { processId, relationship.EntityType, relationship.ViewProperty }));
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


        //public static IViewModelEventCommand<IViewModel, IEvent> SaveEntityCommand(string viewChildProperty,List<EntityViewModelRelationship> rels)
        //{
        //    return new ViewEventCommand<IEntityDetailsViewModel, IUpdatePullEntityWithChanges>(
        //        key: "SaveEntity",
        //        subject: s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),
        //        commandPredicate: new List<Func<IEntityDetailsViewModel, bool>>
        //        {
        //            v => v.ChangeTracking.Count >= 1 && ((dynamic)v).Properties[viewChildProperty].Id != 0
        //        },
        //        //TODO: Make a type to capture this info... i killing it here
        //        messageData: s =>
        //        {
        //            var msg = new ViewEventCommandParameter(
        //                new object[]
        //                {
        //                    ((dynamic) s).Id,
        //                    "Patient",
        //                    "General",
        //                    "Personal Information",
        //                    s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
        //                },
        //                new StateCommandInfo(s.Process.Id, Context.EntityView.Commands.GetEntityView), s.Process,
        //                s.Source);
        //            s.ChangeTracking.Clear();
        //            ((dynamic)s).Properties[viewChildProperty] = null;

        //            return msg;
        //        });
        //}

        //public static IViewModelEventCommand<IViewModel, IEvent> EditEntityCommand(string viewChildProperty, List<EntityViewModelRelationship> rels)
        //{
        //    return new ViewEventCommand<IEntityDetailsViewModel, IUpdatePullEntityWithChanges>(
        //        key: $"EditEntity{EntityType}",
        //        subject: v => v.ChangeTracking.DictionaryChanges,
        //        commandPredicate: new List<Func<IEntityDetailsViewModel, bool>>
        //        {
        //            v => v.ChangeTracking.Count == 1 && ((dynamic)v).Properties[viewChildProperty].Id != 0

        //        },
        //        //TODO: Make a type to capture this info... i killing it here
        //        messageData: v =>
        //        {
        //            foreach (var c in rels)
        //            {
        //                v.ChangeTracking.Add(nameof(c.ChildProperty), ((dynamic)v).Properties[c.ChildProperty]);
        //            }

        //            var msg = new ViewEventCommandParameter(
        //                new object[]
        //                {
        //                    ((dynamic)v).Properties[viewChildProperty].Id,
        //                    v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
        //                },
        //                new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
        //                v.Source);
        //            v.ChangeTracking.Clear();
        //            return msg;
        //        });
        //}

        public static string EntityType { get; set; }

        //public static IViewModelEventCommand<IViewModel, IEvent> CreateEntityCommand(string childProperty, List<EntityViewModelRelationship> rels) 
        //{
        //    return new ViewEventCommand<IEntityDetailsViewModel, IUpdatePullEntityWithChanges> (
        //        key: "CreateEntity",
        //        subject: v => v.ChangeTracking.DictionaryChanges,
        //        commandPredicate: new List<Func<IEntityDetailsViewModel, bool>>
        //        {
        //            v => v.ChangeTracking.Count > 0 && ((dynamic)v).Properties[childProperty].Id == 0

        //        },
        //        //TODO: Make a type to capture this info... i killing it here
        //        messageData: v =>
        //        {
        //            foreach (var c in rels)
        //            {
        //                v.ChangeTracking.Add(nameof(c.ChildProperty), ((dynamic)v).Properties[c.ChildProperty]);
        //            }
        //            var msg = new ViewEventCommandParameter(
        //                new object[]
        //                {
        //                    ((dynamic)v).Properties[childProperty].Id,
        //                    v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
        //                },
        //                new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
        //                v.Source);
        //            v.ChangeTracking.Clear();
        //            return msg;
        //        });
        //}







        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(int processId,string pentity,string parentProperty)
        {
            return new ViewEventSubscription<IEntityDetailsViewModel, ICurrentEntityChanged>(
                processId,
                e => e != null && e.EntityType == pentity,
                new List<Func<IEntityDetailsViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[parentProperty] = e.Entity;
                    v.NotifyPropertyChanged(parentProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> RecieveProcessStateMessage(int processId,string pEntity,  string viewChildProperty) 
        {
            return new ViewEventSubscription<IEntityDetailsViewModel, IProcessStateMessage>(
                processId,
                e => e != null  && e.EntityType == pEntity,
                new List<Func<IEntityDetailsViewModel, IProcessStateMessage, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.State.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityFound(int processId, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityDetailsViewModel, IEntityFound>(
                processId,
                e => e != null && e.EntityType == pEntity,
                new List<Func<IEntityDetailsViewModel, IEntityFound, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityViewWithChangesFound(int processId, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityDetailsViewModel, IEntityWithChangesFound>(
                processId,
                e => e != null && e.EntityType == pEntity,
                new List<Func<IEntityDetailsViewModel, IEntityWithChangesFound, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.Entity;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }


        public static IViewModelEventSubscription<IViewModel, IEvent> EntityViewSetWithChangesLoaded(int processId, string pEntity, string viewChildProperty) 
        {
            return new ViewEventSubscription<IEntityDetailsViewModel, IEntitySetWithChangesLoaded>(
                processId,
                e => e != null && e.EntityType == pEntity,
                new List<Func<IEntityDetailsViewModel, IEntitySetWithChangesLoaded, bool>>(),
                (v, e) =>
                {
                    ((dynamic)v).Properties[viewChildProperty] = e.EntitySet;
                    v.NotifyPropertyChanged(viewChildProperty);
                });
        }

        //public static IViewModelEventPublication<IViewModel, IEvent> IViewStateLoaded(int processId, string childProperty) where TView : IEntityId
        //{
        //    return new ViewEventPublication<IEntityDetailsViewModel, IViewStateLoaded<IEntityDetailsViewModel, IProcessState>>(
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