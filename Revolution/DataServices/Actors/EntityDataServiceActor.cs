﻿using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using EventAggregator;
using EventMessages.Events;
using RevolutionEntities.Process;
using Utilities;

namespace DataServices.Actors
{
  
    public class EntityDataServiceActor<TService>: BaseActor<EntityDataServiceActor<TService>>, IEntityDataServiceActor<TService> where TService:IEntityRequest, IProcessSystemMessage
    {
        private Action<ISystemSource,TService> Action { get; }
       
      
        
        public EntityDataServiceActor(ICreateEntityService cMsg, IDynamicEntityType entityType) : base(cMsg.ActorId,cMsg.Process)
        {
            Action = (Action<ISystemSource,TService>)cMsg.Action;
           
            var processStateInfo = new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, RevolutionData.Context.Entity.Events.EntityRequested), Guid.NewGuid());
            EventMessageBus.Current.GetEvent<TService>(processStateInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == processStateInfo.EventKey)
                .Where(x => x.EntityType == entityType)
                .Subscribe(x => HandledEvent(x));

            EventMessageBus.Current.GetEvent<IUpdateCache>(
                new StateCommandInfo(Process,
                    RevolutionData.Context.CommandFunctions.UpdateCommandData(entityType.Name,
                        RevolutionData.Context.Entity.Commands.UpdateCache), Guid.NewGuid()), Source).Subscribe(x => OnUpdateCache(x));

            EventMessageBus.Current.GetEvent<IEntityDeleted>(
                new StateEventInfo(Process,
                    RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name,
                        RevolutionData.Context.Entity.Events.EntityDeleted), Guid.NewGuid()), Source).Subscribe(x => OnEntityDeleted(x));
            ////////////////////// - no need to do this 
            //if(cMsg.InitialMessage is TService p) HandledEvent(p);
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceActor<TService>>(this,new StateEventInfo(cMsg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(cMsg.ActorType.GetFriendlyName(),RevolutionData.Context.Actor.Events.ActorStarted)), cMsg.Process,Source));
        }

        private void OnEntityDeleted(IEntityDeleted msg)
        {
            if (msg.Entity.EntityType.CachedProperties.ContainsKey("Name") && (msg.Entity.EntityType.CachedProperties["Name"].ContainsKey(msg.Entity.Id) && msg.Entity.EntityType.CachedProperties["Name"][msg.Entity.Id] == msg.Entity.EntityName))
            {
                msg.Entity.EntityType.CachedProperties["Name"].Remove(msg.Entity.Id);
            }
            EventMessageBus.Current.Publish(new CacheUpdated(msg.Entity.EntityType, new StateEventInfo(Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.Entity.EntityType.Name, RevolutionData.Context.Entity.Events.CacheUpdated)), Process, Source));
        }

        private void OnUpdateCache(IUpdateCache msg)
        {
            if (msg.Entity.EntityType.CachedProperties.ContainsKey("Name") && (!msg.Entity.EntityType.CachedProperties["Name"].ContainsKey(msg.Entity.Id) || msg.Entity.EntityType.CachedProperties["Name"][msg.Entity.Id] != msg.Entity.EntityName))
            {
                msg.Entity.EntityType.CachedProperties["Name"].AddOrUpdate(msg.Entity.Id, msg.Entity.EntityName);
            }
            EventMessageBus.Current.Publish(new CacheUpdated(msg.Entity.EntityType, new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventData(msg.Entity.EntityType.Name,RevolutionData.Context.Entity.Events.CacheUpdated)), Process, Source));
        }


        private void HandledEvent(TService msg)
        {
            //TODO:Implement Logging
            // Persist(msg, x => { });//x => Action.Invoke(DbContext, Source, x)
            try
            {
                Task.Run(() => { Action.Invoke(Source,msg);}).ConfigureAwait(false);  
            }
            catch (Exception ex)
            {

                PublishProcesError(msg, ex, typeof (TService));
            }

        }
    }
}