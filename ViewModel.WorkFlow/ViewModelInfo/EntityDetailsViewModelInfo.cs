using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using BootStrapper;
using Common;
using Common.DataEntites;
using Common.Dynamic;
using GenSoft.Entities;
using GenSoft.Interfaces;
using MoreLinq;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModel.WorkFlow;
using ViewModelInterfaces;
using Application = System.Windows.Application;

namespace RevolutionData
{
    
    public class EntityDetailsViewModelInfo
    {
       
        public static ViewModelInfo EntityDetailsViewModel(int processId, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality, string symbol, string description, int priority, List<EntityViewModelRelationship> viewRelationships, List<EntityTypeViewModelCommand> viewCommands, IViewAttributeDisplayProperties displayProperties)
        {
            try
            {
                var parentEntities = viewRelationships.Where(x => x.ParentType != null && DynamicEntityType.DynamicEntityTypes.ContainsKey(x.ParentType)).DistinctBy(x => x.ParentType)
                    .Select(x => new ViewModelEntity(DynamicEntityType.DynamicEntityTypes[x.ParentType],
                        x.ViewParentProperty, x.ParentProperty)).ToList();

                var childEntities = viewRelationships.Where(x => x.ChildType != null && DynamicEntityType.DynamicEntityTypes.ContainsKey(x.ChildType)).DistinctBy(x => x.ChildType)
                    .Select(x => new ViewModelEntity(DynamicEntityType.DynamicEntityTypes[x.ChildType],
                        x.ViewChildProperty, x.ChildProperty)).ToList();

                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new EntityViewInfo($"{entityType.Name}-EntityDetailsViewModel", symbol, description,entityType,ordinality),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        new ViewEventSubscription<IEntityViewModel, IProcessStateMessage>(
                            $"{entityType.Name}-ProcessStateMessage",
                            processId,
                            e => e != null  && e.EntityType.Name == entityType.Name,
                            new List<Func<IEntityViewModel, IProcessStateMessage, bool>>(),
                            (v, e) =>
                            {
                                //Application.Current.Dispatcher.Invoke(() =>
                                //{
                                if (Equals(v.State.Value.Entity, e.State.Entity)) return;
                                v.State.Value = e.State;
                       
                                //});
                    
                    
                            })
                    },
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<IEntityViewModel, ICurrentEntityChanged>(
                            key:$"{entityType.Name}-CurrentEntityChanged",
                            subject:v =>  (IObservable<dynamic>)v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                            subjectPredicate:new List<Func<IEntityViewModel, bool>>{},
                            messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value},new RevolutionEntities.Process.StateEventInfo(s.Process.Id, Context.View.Events.CurrentEntityChanged),s.Process,s.Source)),
                        new ViewEventPublication<IEntityViewModel, ILoadEntitySet>(
                            $"{entityType.Name}-IViewModelIntialized",
                            subject:v => v.ViewModelState as dynamic,
                            subjectPredicate:new List<Func<IEntityViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Intialized},
                            messageData:v => new ViewEventPublicationParameter(new object[] {v.ViewInfo.EntityType},new RevolutionEntities.Process.StateEventInfo(v.Process.Id, Context.View.Events.Intitalized),v.Process,v.Source)),
                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {
                        new ViewEventCommand<IEntityViewModel, IViewRowStateChanged>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<IEntityViewModel, bool>>
                            {
                                //v => v. != null
                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                            messageData: s =>
                            {
                                s.RowState.Value = s.RowState.Value != RowState.Modified?RowState.Modified: RowState.Unchanged;//new ReactiveProperty<RowState>(rowstate != RowState.Modified?RowState.Modified: RowState.Unchanged);

                                return new ViewEventCommandParameter(
                                    new object[] {s,s.RowState.Value},
                                    new RevolutionEntities.Process.StateCommandInfo(s.Process.Id,
                                        Context.Process.Commands.CurrentEntityChanged), s.Process,
                                    s.Source);
                            }),


                    },
                    viewModelType: typeof(IEntityViewModel),
                    orientation: typeof(IBodyViewModel),
                    priority: priority,
                    displayProperties: displayProperties);

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

                parentCommands.AddRange(CreateCustomCommands(viewCommands,viewRelationships));

                viewInfo.Commands.AddRange(parentCommands);



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
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId, relationship.EntityType.Name, relationship.ViewProperty }));
            return res;
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateChildSubscibtions(int processId, ViewModelEntity relationship)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            //res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("RecieveProcessStateMessage").Invoke(null, new object[] { processId,relationship.EntityType.Name, relationship.ViewProperty }));
            //res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityFound").Invoke(null, new object[] { processId, relationship.EntityType.Name, relationship.ViewProperty }));
            //res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityWithChangesFound").Invoke(null, new object[] { processId, relationship.EntityType.Name, relationship.ViewProperty }));
            //res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("EntityWithChangesUpdated").Invoke(null, new object[] { processId, relationship.EntityType.Name, relationship.ViewProperty }));

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

        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateCustomCommands(List<EntityTypeViewModelCommand> viewCommands, List<EntityViewModelRelationship> parentEntities)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
            foreach (var cmd in viewCommands)
            {
                res.Add(ViewModelInfoExtensions.CreateCustomCommand<IEntityViewModel>(cmd.ViewModelCommands, parentEntities));
            }
            return res;
        }


        public static IDynamicEntityType entityType { get; set; }

        
        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(int processId,string pentity,string parentProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, ICurrentEntityChanged>(
                $"{pentity}-CurrentEntityChanged",
                processId,
                e => e != null && e.EntityType?.Name == pentity,
                new List<Func<IEntityViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        v.ParentEntities.AddOrUpdate(e.Entity);
                        //v.NotifyPropertyChanged(parentProperty);
                    });
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> RecieveProcessStateMessage(int processId,string pEntity,  string viewChildProperty) 
        {
            return new ViewEventSubscription<IEntityViewModel, IProcessStateMessage>(
                $"{pEntity}-ProcessStateMessage",
                processId,
                e => e != null  && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IProcessStateMessage, bool>>(),
                (v, e) =>
                {
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                       if (Equals(v.State.Value.Entity, e.State.Entity)) return;
                        v.State.Value = e.State;
                       
                    //});
                    
                    
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityFound(int processId, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, IEntityFound>(
                $"{pEntity}-IEntityFound",
                processId,
                e => e != null && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IEntityFound, bool>>(),
                (v, e) =>
                {
                    v.State.Value.Entity = e.Entity;
                    v.NotifyPropertyChanged("State");
                });
        }

        //public static IViewModelEventSubscription<IViewModel, IEvent> EntityWithChangesFound(int processId, string pEntity, string viewChildProperty)
        //{
        //    return new ViewEventSubscription<IEntityViewModel, IEntityWithChangesFound>(
        //        processId,
        //        e => e != null && e.EntityType.Name == pEntity,
        //        new List<Func<IEntityViewModel, IEntityWithChangesFound, bool>>(),
        //        (v, e) =>
        //        {
        //            v.State.Value.Entity = e.Entity;
        //            v.NotifyPropertyChanged("State");
        //        });
        //}

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityWithChangesUpdated(int processId, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, IEntityWithChangesUpdated>(
                $"{pEntity}-IEntityWithChangesUpdated",
                processId,
                e => e != null && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IEntityWithChangesUpdated, bool>>(),
                (v, e) =>
                {
                    v.State.Value.Entity = v.State.Value.Entity.ApplyChanges(e.Changes);
                    v.NotifyPropertyChanged("State");
                });
        }




        //public static IViewModelEventPublication<IViewModel, IEvent> IViewStateLoaded(int processId, string childProperty) where TView : IEntityId
        //{
        //    return new ViewEventPublication<IEntityViewModel, IViewStateLoaded<IEntityViewModel, IProcessState>>(
        //        key: "ViewStateLoaded",
        //        subject: v => v.WhenAny(),
        //        subjectPredicate: new List<Func<IEntityViewModel, bool>>
        //        {
        //            v => v.State != null
        //        },
        //        messageData: s => new ViewEventPublicationParameter(new object[] { s, s.State.Value }, new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process, s.Source)),;
        //}



    }

}