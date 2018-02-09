using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using EventAggregator;
using EventMessages.Events;
using StateEventInfo = RevolutionEntities.Process.StateEventInfo;

namespace DataServices.Actors
{
    public class EntityDataServiceManager : BaseSupervisor<EntityDataServiceManager>, IEntityDataServiceManager
    {

        private IUntypedActorContext ctx = null;

        public EntityDataServiceManager(ISystemProcess process) : base(process)
        {
            ctx = Context;
            EventMessageBus.Current.GetEvent<IEntityRequest>(Source).Subscribe(handleEntityRequest);
            EventMessageBus.Current.Publish(
                new ServiceStarted<IEntityDataServiceManager>(this,
                    new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted), process, Source),
                Source);


        }

        private void handleEntityRequest(IEntityRequest entityRequest)
        {
            try
            {
                var type = "IDynamicEntity"; //entityRequest.EntityType;

                CreateEntityActors(type, "EntityDataServiceSupervisor{0}",
                    entityRequest.EntityType, entityRequest.Process, entityRequest);

            }
            catch (Exception)
            {

                throw;
            }


        }

        private void CreateEntityActors(string classType, string actorName,IDynamicEntityType entityType , ISystemProcess process,
            IProcessSystemMessage msg)
        {
            var child = ctx.Child(string.Format(actorName, entityType.Name));
            if (!Equals(child, ActorRefs.Nobody))
            {
                child.Tell(msg);
            }
            else
            { 
                try
                {
                    Task.Run(() =>
                    {
                        ctx.ActorOf(Props.Create<EntityDataServiceSupervisor>(entityType, process, msg),string.Format(actorName, entityType.Name));
                    });
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    EventMessageBus.Current.Publish(new ProcessEventFailure(failedEventType: msg.GetType(),
                            failedEventMessage: msg,
                            expectedEventType: typeof(IServiceStarted<EntityDataServiceSupervisor>),
                            exception: ex,
                            source: Source,
                            processInfo: new StateEventInfo(msg.Process, RevolutionData.Context.Process.Events.Error)),
                        Source);
                }


            }

        }
    }
}