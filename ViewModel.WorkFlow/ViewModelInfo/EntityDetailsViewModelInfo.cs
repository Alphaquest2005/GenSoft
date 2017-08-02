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

namespace RevolutionData
{
    
    public class EntityDetailsViewModelInfo<TView> where TView : IEntityView
    {
        public static ViewModelInfo EntityDetailsViewModel(int processId, string symbol, string description, int priority, List<EntityViewModelRelationship> parentEntities)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new ViewInfo($"{typeof(TView).Name}EntityDetailsViewModel", symbol, description),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        new ViewEventSubscription<IEntityDetailsViewModel<TView>, IUpdateProcessStateList<TView>>(
                            3,
                            e => e != null,
                            new List<Func<IEntityDetailsViewModel<TView>, IUpdateProcessStateList<TView>, bool>>(),
                            (v,e) =>
                            {
                                if (v.State.Value == e.State) return;
                                v.State.Value = e.State;
                            }),

                        new ViewEventSubscription<IEntityDetailsViewModel<TView>, IEntityFound<TView>>(
                            3,
                            e => e != null,
                            new List<Func<IEntityDetailsViewModel<TView>, IEntityFound<TView>, bool>>(),
                            (v, e) =>
                            {
                                v.State.Value = new ProcessState<TView>(e.Process,e.Entity, new StateInfo(e.ProcessInfo.ProcessId, e.ProcessInfo.State));
                            }),

                    },
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<IEntityDetailsViewModel<TView>, IViewStateLoaded<IEntityDetailsViewModel<TView>,IProcessState<TView>>>(
                            key:"ViewStateLoaded",
                            subject:v => v.State,
                            subjectPredicate:new List<Func<IEntityDetailsViewModel<TView>, bool>>
                            {
                                v => v.State != null
                            },
                            messageData:s => new ViewEventPublicationParameter(new object[] {s,s.State.Value},new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded),s.Process,s.Source)),

                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                    {


                        new ViewEventCommand<IEntityDetailsViewModel<TView>, IViewRowStateChanged<TView>>(
                            key:"EditEntity",
                            commandPredicate:new List<Func<IEntityDetailsViewModel<TView>, bool>>
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

                        new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdatePullEntityWithChanges>(
                            key:"SaveEntity",
                            subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),
                            commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
                            {
                                v => v.ChangeTracking.Count >= 1
                                     && v.State.Value.Entity.Id != 0
                            },
                            //TODO: Make a type to capture this info... i killing it here
                            messageData: s =>
                            {
                                var msg = new ViewEventCommandParameter(
                                    new object[]
                                    {
                                        ((dynamic)s).Id,
                                        "Patient",
                                        "General",
                                        "Personal Information",
                                        s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
                                    },
                                    new StateCommandInfo(s.Process.Id, Context.EntityView.Commands.GetEntityView), s.Process,
                                    s.Source);
                                s.ChangeTracking.Clear();
                                s.State.Value = null;

                                return msg;
                            }),



                    },
                    viewModelType: typeof(IEntityDetailsViewModel<TView>),
                    orientation: typeof(IBodyViewModel),
                    priority: priority);

                var parentSubscriptions = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
                var parentCommands = new List<IViewModelEventCommand<IViewModel, IEvent>>();
                foreach (var p in parentEntities)
                {
                    parentSubscriptions.AddRange(CreateParentEntitySubscibtion(processId, p.ParentType, p.CurrentParentEntity));

                }
                viewInfo.Subscriptions.AddRange(parentSubscriptions);

                //parentCommands.AddRange(CreateParentEntityCommands(parentEntities.Select(x => x.ChildProperty).ToList()));


                return viewInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        //private static List<IViewModelEventCommand<IViewModel, IEvent>> CreateParentEntityCommands(List<string> childProperty)
        //{
        //    List<IViewModelEventCommand<IViewModel, IEvent>> res = new List<IViewModelEventCommand<IViewModel, IEvent>>();
        //    res.Add(CreateEntityCommand(childProperty));
        //    res.Add(EditEntityCommand(childProperty));
        //    return res;
        //}


        //public static IViewModelEventCommand<IViewModel, IEvent> CreateEntityCommand(List<string> childProperties)
        //{
        //    return  new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdateEntityViewWithChanges<TView>>(
        //        key: $"EditEntity{typeof(TView).Name}",
        //        subject: v => v.ChangeTracking.DictionaryChanges,
        //        commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
        //        {
        //            v => v.ChangeTracking.Count == 1 && v.CurrentEntity.Value.Id != 0

        //        },
        //        //TODO: Make a type to capture this info... i killing it here
        //        messageData: v =>
        //        {
        //            foreach (var childProperty in childProperties)
        //            {
        //                v.ChangeTracking.Add(nameof(childProperty), ((dynamic)v).Properties["childProperty"]);
        //            }
                    
        //            var msg = new ViewEventCommandParameter(
        //                new object[]
        //                {
        //                    v.CurrentEntity.Value.Id,
        //                    v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
        //                },
        //                new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
        //                v.Source);
        //            v.ChangeTracking.Clear();
        //            return msg;
        //        });
        //}

        //public static IViewModelEventCommand<IViewModel, IEvent> EditEntityCommand(List<string> childProperties)
        //{
        //    return new ViewEventCommand<IEntityDetailsViewModel<TView>, IUpdateEntityViewWithChanges<TView>>(
        //        key: "CreateEntity",
        //        subject: v => v.ChangeTracking.DictionaryChanges,
        //        commandPredicate: new List<Func<IEntityDetailsViewModel<TView>, bool>>
        //        {
        //            v => v.ChangeTracking.Count > 0 && v.CurrentEntity.Value.Id == 0

        //        },
        //        //TODO: Make a type to capture this info... i killing it here
        //        messageData: v =>
        //        {
        //            foreach (var childProperty in childProperties)
        //            {
        //                v.ChangeTracking.Add(nameof(childProperty), ((dynamic)v).Properties["childProperty"]);
        //            }
        //            var msg = new ViewEventCommandParameter(
        //                new object[]
        //                {
        //                    v.CurrentEntity.Value.Id,
        //                    v.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)
        //                },
        //                new StateCommandInfo(v.Process.Id, Context.EntityView.Commands.GetEntityView), v.Process,
        //                v.Source);
        //            v.ChangeTracking.Clear();
        //            return msg;
        //        });
        //}

        private static List<IViewModelEventSubscription<IViewModel, IEvent>> CreateParentEntitySubscibtion(int processId, Type parentEntity, string parentProperty)
        {
            var res = new List<IViewModelEventSubscription<IViewModel, IEvent>>();
            
            res.Add((IViewModelEventSubscription<IViewModel, IEvent>) typeof(EntityDetailsViewModelInfo<TView>).GetMethod("ParentCurrentEntityChanged").MakeGenericMethod(parentEntity).Invoke(null, new object[] { processId, parentProperty}));
            return res;
        }





        public static IViewModelEventSubscription<IViewModel, IEvent> ParentCurrentEntityChanged<PEntity>(int processId,string parentProperty)
        {
            return new ViewEventSubscription<IEntityDetailsViewModel<TView>, ICurrentEntityChanged<PEntity>>(
                processId,
                e => e != null,
                new List<Func<IEntityDetailsViewModel<TView>, ICurrentEntityChanged<PEntity>, bool>>(),
                (v, e) =>
                {
                    ((Expando)v).Properties[parentProperty] = e.Entity;
                });
        }


       
    }

}