using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
    
    public class EntityViewCacheViewModelInfo
    {
        public static ViewModelInfo CacheViewModel(int processId, string entityType)
        {
            return new ViewModelInfo
                (
                processId: processId,
                viewInfo: new ViewInfo($"{entityType}CacheViewModel", "", ""),
                subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IEntityListCacheViewModel, IEntitySetWithChangesLoaded>(
                        processId: processId,
                        eventPredicate: e => e.Changes.Count == 0 && e.EntitySet.First().EntityType == entityType,
                        actionPredicate: new List<Func<IEntityListCacheViewModel, IEntitySetWithChangesLoaded, bool>>(),
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

                    new ViewEventSubscription<IEntityListCacheViewModel, IEntityWithChangesUpdated>(
                        processId: processId,
                        eventPredicate: e => e.Changes.Count > 0 && e.Entity.EntityType == entityType,
                        actionPredicate: new List<Func<IEntityListCacheViewModel, IEntityWithChangesUpdated, bool>>(),
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

                     new ViewEventSubscription<IEntityListCacheViewModel, ICurrentEntityChanged>(
                            3,
                            e => e != null && e.Entity.EntityType == entityType,
                            new List<Func<IEntityListCacheViewModel, ICurrentEntityChanged, bool>>(),
                            (v,e) => v.CurrentEntity.Value = e.Entity),


                },
                publications: new List<IViewModelEventPublication<IViewModel, IEvent>> {},
                commands: new List<IViewModelEventCommand<IViewModel, IEvent>> {},
                viewModelType: typeof (IEntityListCacheViewModel),
                orientation: typeof (ICacheViewModel),
                priority:0);
        }



        private static void UpdateEntitySet(IEntityListCacheViewModel cacheViewModel,
            IEntityWithChangesUpdated msg)
        {
            var existingEntity = cacheViewModel.EntitySet.Value.FirstOrDefault(x => x.Id == msg.Entity.Id);
            if (existingEntity != null) cacheViewModel.EntitySet.Value.Remove(existingEntity);

            cacheViewModel.EntitySet.Value.Add(msg.Entity);
            cacheViewModel.EntitySet.Value.Reset();

        }

        private static void ReloadEntitySet(IEntityListCacheViewModel v, IEntitySetWithChangesLoaded e)
        {
            v.EntitySet.Value.Clear();
            v.EntitySet.Value.AddRange(e.EntitySet);
            v.EntitySet.Value.Reset();
        }

       
    }
}