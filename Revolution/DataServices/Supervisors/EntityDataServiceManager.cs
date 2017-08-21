using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Akka.Actor;
using Akka.Event;
using Akka.IO;
using Akka.Routing;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;
using EventMessages.Events;
using RevolutionEntities.Process;
using Utilities;
using ViewMessages;

namespace DataServices.Actors
{
    public class EntityDataServiceManager : BaseSupervisor<EntityDataServiceManager>
    {
       
        private IUntypedActorContext ctx = null;
        public EntityDataServiceManager(ISystemProcess process) : base(process)
        {
            ctx = Context;
            EventMessageBus.Current.GetEvent<IEntityRequest>(Source).Subscribe(handleEntityRequest);
        }

        private void handleEntityRequest(IEntityRequest entityRequest)
        {
            try
            {
                var type = "IDynamicEntity";//entityRequest.EntityType;

                CreateEntityActors(type,  "{0}EntityDataServiceSupervisor",
                        entityRequest.Process, entityRequest);
                
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void CreateEntityActors(string classType,  string actorName, ISystemProcess process, IProcessSystemMessage msg)
        {
            var child = ctx.Child(string.Format(actorName, classType));
            if (!Equals(child, ActorRefs.Nobody)) return;
            
            try
            {
                Task.Run(() => { ctx.ActorOf(Props.Create<EntityDataServiceSupervisor>(process, msg), string.Format(actorName, classType)); });
            }
            catch (Exception ex)
            {
                Debugger.Break();
                EventMessageBus.Current.Publish(new ProcessEventFailure(failedEventType: msg.GetType(),
                    failedEventMessage: msg,
                    expectedEventType: typeof(IServiceStarted<EntityDataServiceSupervisor>),
                    exception: ex,
                    source: Source, processInfo: new StateEventInfo(msg.Process.Id, RevolutionData.Context.Process.Events.Error)), Source);
            }

        }
    }

}