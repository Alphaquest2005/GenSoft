using System;
using SystemInterfaces;
using Actor.Interfaces;
using EventAggregator;
using EventMessages.Events;
using RevolutionEntities.Process;

namespace DataServices.Actors
{
  
    public class EntityDataServiceActor<TService>: BaseActor<EntityDataServiceActor<TService>>, IEntityDataServiceActor<TService> where TService:IProcessSystemMessage
    {
        private Action<ISystemSource,TService> Action { get; }
       
      
        
        public EntityDataServiceActor(ICreateEntityService msg, IProcessSystemMessage firstMessage) : base(msg.Process)
        {
            Action = (Action<ISystemSource,TService>)msg.Action;
            // Command<TService>(m => HandledEvent(m));
            if(firstMessage is TService) HandledEvent((TService)firstMessage);
            EventMessageBus.Current.GetEvent<TService>(Source).Subscribe(x => HandledEvent(x));
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceActor<TService>>(this,new StateEventInfo(msg.Process.Id, RevolutionData.Context.Actor.Events.ActorStarted), msg.Process,Source), Source);
        }


        private void HandledEvent(TService msg)
        {
            //TODO:Implement Logging
            // Persist(msg, x => { });//x => Action.Invoke(DbContext, Source, x)
            try
            {
                Action.Invoke(Source,msg);
            }
            catch (Exception ex)
            {

                PublishProcesError(msg, ex, typeof (TService));
            }

        }
    }
}