using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities.Process;
using StateEventInfo = RevolutionEntities.Process.StateEventInfo;
using Utilities;

namespace DataServices.Actors
{
    public class EntityDataServiceManager : BaseSupervisor<EntityDataServiceManager>, IEntityDataServiceManager
    {

        private static ConcurrentDictionary<IDynamicEntityType, EntityDataServiceSupervisor> existingSupervisors = new ConcurrentDictionary<IDynamicEntityType, EntityDataServiceSupervisor>();

        public EntityDataServiceManager(ISystemProcess process) : base(process)
        {
            var stateCommandInfo = new StateCommandInfo(process, RevolutionData.Context.Entity.Commands.EntityRequest, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<IEntityRequest>(stateCommandInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == stateCommandInfo.EventKey)
                .Subscribe(handleEntityRequest);


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
           
                try
                {
                    Task.Run(() =>
                    {
                        if(!existingSupervisors.ContainsKey(entityType)) existingSupervisors.AddOrUpdate(entityType, new EntityDataServiceSupervisor(entityType, process, msg));
                    }).ConfigureAwait(false);
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