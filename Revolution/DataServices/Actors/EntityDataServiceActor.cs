using System;
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
  
    public class EntityDataServiceActor<TService>: BaseActor<EntityDataServiceActor<TService>>, IEntityDataServiceActor<TService> where TService:IEntityRequest
    {
        private Action<ISystemSource,TService> Action { get; }
       
      
        
        public EntityDataServiceActor(ICreateEntityService msg, IDynamicEntityType entityType, IProcessSystemMessage firstMessage) : base(msg.ActorId,msg.Process)
        {
            Action = (Action<ISystemSource,TService>)msg.Action;
            if(firstMessage is TService service) HandledEvent(service);


            var processStateInfo = new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, RevolutionData.Context.Entity.Events.EntityRequested), Guid.NewGuid());
            EventMessageBus.Current.GetEvent<TService>(processStateInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == processStateInfo.EventKey)
                .Where(x => x.EntityType == entityType)
                .Subscribe(x => HandledEvent(x));
            
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceActor<TService>>(this,new StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventData(msg.ActorType.GetFriendlyName(),RevolutionData.Context.Actor.Events.ActorStarted)), msg.Process,Source), Source);
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