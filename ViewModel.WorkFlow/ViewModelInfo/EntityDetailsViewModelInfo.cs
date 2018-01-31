using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using DomainUtilities;
using GenSoft.Entities;
using MoreLinq;
using Reactive.Bindings;
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
       
        public static ViewModelInfo EntityDetailsViewModel(ISystemProcess process, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality, string symbol, string description, int priority, List<EntityViewModelRelationship> viewRelationships, List<EntityTypeViewModelCommand> viewCommands, IViewAttributeDisplayProperties displayProperties)
        {
            try
            {
                var parentEntities = viewRelationships.Where(x => x.ParentType != null && DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(x.ParentType) != null).DistinctBy(x => x.ParentType)
                    .Select(x => new ViewModelEntity(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(x.ParentType),
                        x.ViewParentProperty, x.ParentProperty)).ToList();

                var childEntities = viewRelationships.Where(x => x.ChildType != null && DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(x.ChildType) != null).DistinctBy(x => x.ChildType)
                    .Select(x => new ViewModelEntity(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(x.ChildType),
                        x.ViewChildProperty, x.ChildProperty)).ToList();

                var viewInfo = new ViewModelInfo
                (
                    process: process,
                    viewInfo: new EntityViewInfo($"{entityType.Name}-EntityDetailsViewModel", symbol, description,entityType,ordinality),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        new ViewEventSubscription<IEntityViewModel, IProcessStateMessage>(
                            $"{entityType.Name}-ProcessStateMessage",
                            process,
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
                       
                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {
                        new ViewEventCommand<IEntityViewModel, IViewRowStateChanged>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<IEntityViewModel, bool>>
                            {
                                //v => v. != null
                            },
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel>>(),

                            messageData: s =>
                            {
                                s.RowState.Value = s.RowState.Value != RowState.Modified?RowState.Modified: RowState.Unchanged;

                                return new ViewEventCommandParameter(
                                    new object[] {s,s.RowState.Value},
                                    new RevolutionEntities.Process.StateCommandInfo(s.Process,
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
                    parentSubscriptions.AddRange(CreateChildSubscibtions(process,p));
                    //parentPublications.AddRange(CreatePublications(p));
                    //parentCommands.AddRange(CreateCommands(p));
                }

                foreach (var p in parentEntities)
                {
                    parentSubscriptions.AddRange(CreateParentSubscibtions(process, p));
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

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentSubscibtions(ISystemProcess processId, ViewModelEntity relationship)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("ParentCurrentEntityChanged").Invoke(null, new object[] { processId, relationship.EntityType.Name, relationship.ViewProperty }));
            return res;
        }

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateChildSubscibtions(ISystemProcess processId, ViewModelEntity relationship)
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

        
        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(ISystemProcess process,string pentity,string parentProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, ICurrentEntityChanged>(
                $"{pentity}-CurrentEntityChanged",
                process,
                e => e != null && e.EntityType?.Name == pentity,
                new List<Func<IEntityViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        v.ParentEntities.AddOrUpdate(e.Entity);
                        
                    });
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> RecieveProcessStateMessage(ISystemProcess process,string pEntity,  string viewChildProperty) 
        {
            return new ViewEventSubscription<IEntityViewModel, IProcessStateMessage>(
                $"{pEntity}-ProcessStateMessage",
                process,
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

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityFound(ISystemProcess process, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, IEntityFound>(
                $"{pEntity}-IEntityFound",
                process,
                e => e != null && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IEntityFound, bool>>(),
                (v, e) =>
                {
                    v.State.Value.Entity = e.Entity;
                    v.NotifyPropertyChanged("State");
                });
        }


        public static IViewModelEventSubscription<IViewModel, IEvent> EntityWithChangesUpdated(ISystemProcess process, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, IEntityWithChangesUpdated>(
                $"{pEntity}-IEntityWithChangesUpdated",
                process,
                e => e != null && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IEntityWithChangesUpdated, bool>>(),
                (v, e) =>
                {
                    v.State.Value.Entity = v.State.Value.Entity.ApplyChanges(e.Changes);
                    v.NotifyPropertyChanged("State");
                });
        }

    }

}