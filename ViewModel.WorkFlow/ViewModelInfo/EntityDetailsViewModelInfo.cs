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
using Common.Dynamic;
using DomainMessages;
using GenSoft.Entities;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModel.WorkFlow;
using ViewModelInterfaces;

namespace RevolutionData
{
    
    public class EntityDetailsViewModelInfo
    {
       
        public static ViewModelInfo EntityDetailsViewModel(int processId, IDynamicEntityType entityType,  string symbol, string description, int priority, List<ViewModelEntity> parentEntities, List<ViewModelEntity> childEntities, List<EntityViewModelCommands> viewCommands)
        {
            try
            {
                
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new EntityViewInfo($"EntityDetailsViewModel-{entityType.Name}", symbol, description,entityType),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>{},
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>{},
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
                                    new StateCommandInfo(s.Process.Id,
                                        Context.Process.Commands.CurrentEntityChanged), s.Process,
                                    s.Source);
                            }),

                        //new ViewEventCommand<IEntityViewModel, IGetEntityWithChanges>(
                        //    key:"FindEntity",
                        //    commandPredicate:new List<Func<IEntityViewModel, bool>>
                        //    {
                        //        //v => v. != null
                        //    },
                        //    subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),

                        //    messageData: s =>
                        //    {
                                

                        //        return new ViewEventCommandParameter(
                        //            new object[] {s.State.Value.Entity.EntityType, s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)},
                        //            new StateCommandInfo(s.Process.Id,
                        //                Context.Process.Commands.CurrentEntityChanged), s.Process,
                        //            s.Source);
                        //    }),
                    },
                    viewModelType: typeof(IEntityViewModel),
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

                parentCommands.AddRange(CreateCustomCommands(viewCommands));

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
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>)typeof(EntityDetailsViewModelInfo).GetMethod("RecieveProcessStateMessage").Invoke(null, new object[] { processId,relationship.EntityType.Name, relationship.ViewProperty }));
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

        private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateCustomCommands(List<EntityViewModelCommands> viewCommands)
        {
            List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
            foreach (var cmd in viewCommands)
            {
                res.Add(ViewModelInfoExtensions.CreateCustomCommand<IEntityViewModel>(cmd.Name, cmd.ViewModelCommands));
            }
            return res;
        }


        public static IDynamicEntityType entityType { get; set; }

        
        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged(int processId,string pentity,string parentProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, ICurrentEntityChanged>(
                processId,
                e => e != null && e.EntityType.Name == pentity,
                new List<Func<IEntityViewModel, ICurrentEntityChanged, bool>>(),
                (v, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((dynamic) v).Properties[parentProperty] = e.Entity;
                        v.NotifyPropertyChanged(parentProperty);
                    });
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> RecieveProcessStateMessage(int processId,string pEntity,  string viewChildProperty) 
        {
            return new ViewEventSubscription<IEntityViewModel, IProcessStateMessage>(
                processId,
                e => e != null  && e.EntityType.Name == pEntity,
                new List<Func<IEntityViewModel, IProcessStateMessage, bool>>(),
                (v, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (v.State.Value.Entity == e.State.Entity) return;
                        v.State.Value = e.State;
                       
                    });
                    
                    
                });
        }

        public static IViewModelEventSubscription<IViewModel, IEvent> EntityFound(int processId, string pEntity, string viewChildProperty)
        {
            return new ViewEventSubscription<IEntityViewModel, IEntityFound>(
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