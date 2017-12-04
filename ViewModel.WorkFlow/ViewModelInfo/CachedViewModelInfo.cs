using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using GenSoft.Entities;
using JB.Collections.Reactive;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using Action = System.Action;
using Application = System.Windows.Application;
using IEvent = SystemInterfaces.IEvent;
using IViewModel = ViewModel.Interfaces.IViewModel;

namespace RevolutionData
{
    
    public class CachedViewModelInfo
    {
        public static ViewModelInfo CachedViewModel(int processId, IDynamicEntityType entityType, EntityRelationshipOrdinality ordinality, string symbol, string description, int priority, List<EntityViewModelRelationship> viewRelationships, List<EntityTypeViewModelCommand> viewCommands, IViewAttributeDisplayProperties displayProperties)
        {
            try
            {
                var viewInfo = new ViewModelInfo
                (
                    processId: processId,
                    viewInfo: new EntityViewInfo($"{entityType.Name}-CachedViewModel", symbol, description,entityType, ordinality),
                    subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                    {
                        //new ViewEventSubscription<ICacheViewModel, IViewModelIntialized>(
                        //    3,
                        //    e => e.ViewModel != null,
                        //    new List<Func<ICacheViewModel, IViewModelIntialized, bool>>(),
                        //    (v,e) =>
                        //    {
                        //        if (e.ViewModel.ViewInfo.Name != v.ViewInfo.Name) return;
                        //        v.EntitySet.Value.Add(new DynamicEntity(entityType,0){EntityName = "Create New..."});
                        //        v.EntitySet.Value.Reset();
                        //    }),

                        new ViewEventSubscription<ICacheViewModel, IUpdateProcessStateList>(
                            $"{entityType.Name}-IEntityWithChangesUpdated",
                            processId,
                            e => e.EntityType == entityType,
                            new List<Func<ICacheViewModel, IUpdateProcessStateList, bool>>(),
                            (v,e) =>
                            {
                                if (v.State.Value == e.State) return;
                                v.State.Value = e.State;
                                
                            }),

                        
                        new ViewEventSubscription<ICacheViewModel, IEntityWithChangesUpdated>(
                            key:$"{entityType.Name}-IEntityWithChangesUpdated",
                            processId: processId,
                            eventPredicate: e => e.Changes.Count > 0 && e.EntityType == entityType,
                            actionPredicate: new List<Func<ICacheViewModel, IEntityWithChangesUpdated, bool>>(),
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

                      

                    },
                    publications: new List<IViewModelEventPublication<IViewModel, IEvent>>
                    {
                        new ViewEventPublication<ICacheViewModel, IViewStateLoaded<ICacheViewModel,IProcessStateList>>(
                            key:"ViewStateLoaded",
                            subject:v => v.State,
                            subjectPredicate:new List<Func<ICacheViewModel, bool>>
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
                                    new RevolutionEntities.Process.StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded), s.Process,
                                    s.Source);
                            }),

                        new ViewEventPublication<ICacheViewModel, ILoadEntitySet>(
                            $"{entityType.Name}-IViewModelIntialized",
                            subject:v => v.ViewModelState as dynamic,
                            subjectPredicate:new List<Func<ICacheViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Intialized},
                            messageData:v => new ViewEventPublicationParameter(new object[] {v.ViewInfo.EntityType},new RevolutionEntities.Process.StateEventInfo(v.Process.Id, Context.View.Events.Intitalized),v.Process,v.Source)),


                    },
                    commands: new List<IViewModelEventCommand<IViewModel, IEvent>>{},
                    viewModelType: typeof(ICacheViewModel),
                    orientation: typeof(ICacheViewModel),
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



        
        
        private static void UpdateEntitySet(ICacheViewModel cachedViewModel, IEntityWithChangesUpdated msg)
        {
            var res = cachedViewModel.EntitySet.Value.ToList();
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
            cachedViewModel.EntitySet.Value = new ObservableList<IDynamicEntity>(res);
            cachedViewModel.EntitySet.Value.Reset();

            cachedViewModel.RowState.Value = RowState.Unchanged;


        }
       
    }


}

