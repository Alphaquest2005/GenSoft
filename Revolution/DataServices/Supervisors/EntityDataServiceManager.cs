using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Event;
using Akka.IO;
using Akka.Routing;
using Common.DataEntites;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;
using EventMessages.Commands;
using EventMessages.Events;
using JB.Collections.Reactive;
using RevolutionEntities.Process;
using Utilities;
using ViewMessages;

namespace DataServices.Actors
{
    public class EntityDataServiceManager : BaseSupervisor<EntityDataServiceManager>, IEntityDataServiceManager
    {
       
        private IUntypedActorContext ctx = null;
        public EntityDataServiceManager(ISystemProcess process) : base(process)
        {
            ctx = Context;
            EventMessageBus.Current.GetEvent<IEntityRequest>(Source).Subscribe(handleEntityRequest);
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceManager>(this, new StateEventInfo(process.Id, RevolutionData.Context.Actor.Events.ActorStarted), process, Source), Source);

            EventMessageBus.Current.Publish(
                new LoadEntitySet(
                    new DynamicEntityType("Test Entity", "Test Entities", new List<IEntityKeyValuePair>(),
                        new Dictionary<string, List<dynamic>>(),
                        new ObservableDictionary<string, Dictionary<int, dynamic>>(),
                        new ObservableDictionary<string, string>()),
                    new StateCommandInfo(process.Id,
                        RevolutionData.Context.Entity.Commands.LoadEntitySetWithChanges), process,
                    Source), Source);
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
            if (!Equals(child, ActorRefs.Nobody))
            {
                child.Tell(msg);
            }
            else
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