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
       
      
        
        public EntityDataServiceActor(ICreateEntityService msg, IDynamicEntityType entityType, IProcessSystemMessage firstMessage) : base(msg.Process)
        {
            Action = (Action<ISystemSource,TService>)msg.Action;
            if(firstMessage is TService service) HandledEvent(service);
            EventMessageBus.Current.GetEvent<TService>(new StateEventInfo(Process,new StateEvent("EntityDataServiceActor","Get service events", "")), Source).Where(x => x.EntityType == entityType).Subscribe(x => HandledEvent(x));
            
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceActor<TService>>(this,new StateEventInfo(msg.Process, RevolutionData.Context.EventFunctions.UpdateEventStatus(msg.ActorType.GetFriendlyName(),RevolutionData.Context.Actor.Events.ActorStarted)), msg.Process,Source), Source);
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