using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SystemInterfaces;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{

    
    public class EntityCacheViewModelInfo
    {
        
        public static ViewModelInfo CacheViewModel(int processId, string entityType)
        {
            return new ViewModelInfo
                (
                processId: processId,
                viewInfo: new ViewInfo($"{entityType}CacheViewModel","",""), 
                subscriptions: new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IEntityListCacheViewModel, IEntitySetLoaded>(
                        processId: processId,
                        eventPredicate: e => e != null && e.EntityType == entityType,
                        actionPredicate: new List<Func<IEntityListCacheViewModel, IEntitySetLoaded, bool>>(),
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

                    new ViewEventSubscription<IEntityListCacheViewModel, IEntityUpdated>(
                        processId: processId,
                        eventPredicate: e => e != null && e.EntityType == entityType,
                        actionPredicate: new List<Func<IEntityListCacheViewModel, IEntityUpdated, bool>>(),
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
                            e => e != null && e.EntityType == entityType,
                            new List<Func<IEntityListCacheViewModel, ICurrentEntityChanged, bool>>(),
                            (v,e) => v.CurrentEntity.Value = e.Entity),

                },
                publications: new List<IViewModelEventPublication<IViewModel, IEvent>> {},
                commands: new List<IViewModelEventCommand<IViewModel, IEvent>>
                {
                    
                },
                viewModelType: typeof(IEntityCacheViewModel),
                orientation: typeof (ICacheViewModel),
                priority:0);
        }


       
        private static void UpdateEntitySet(IEntityListCacheViewModel cacheViewModel,
            IEntityUpdated msg)
        {
            var existingEntity = cacheViewModel.EntitySet.Value.FirstOrDefault(x => x.Id == msg.Entity.Id);
            if (existingEntity != null) cacheViewModel.EntitySet.Value.Remove(existingEntity);

            cacheViewModel.EntitySet.Value.Add(msg.Entity);
            cacheViewModel.EntitySet.Value.Reset();

        }

        private static void ReloadEntitySet(IEntityListCacheViewModel v, IEntitySetLoaded e)
        {
            v.EntitySet.Value.Clear();
            v.EntitySet.Value.AddRange(e.Entities);
            v.EntitySet.Value.Reset();
        }


       

   

    }


}