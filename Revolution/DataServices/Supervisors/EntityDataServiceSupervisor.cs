using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;

namespace DataServices.Actors
{
    public class EntityDataServiceSupervisor : BaseSupervisor<EntityDataServiceSupervisor>, IEntityDataServiceSupervisor
    {

        private static readonly Action<ICreateEntity> CreateAction = (x) => x.CreateEntity();
        private static readonly Action<IDeleteEntity> DeleteAction = (x) => x.DeleteEntity();
        private static readonly Action<IUpdateEntityWithChanges> UpdateAction = (x) => x.UpdateEntity();
        private static readonly Action<IGetEntityById> GetEntityByIdAction = (x) => x.GetEntity();
        private static readonly Action<IGetEntityWithChanges> GetEntityWithChangesAction = (x) => x.GetEntity();

        private static readonly Action<ILoadEntitySet> LoadEntitySet = (x) => x.LoadEntitySet();
        private static readonly Action<IGetEntitySetWithChanges> LoadEntitySetWithChanges = (x) => x.LoadEntitySetWithChanges();
        private static readonly Action<ILoadEntitySetWithFilter> LoadEntitySetWithFilter = (x) => x.LoadEntitySet();
        private static readonly Action<ILoadEntitySetWithFilterWithIncludes> LoadEntitySetWithFilterWithIncludes = (x) => x.LoadEntitySet();

        

        readonly Dictionary<Type, object> entityEvents =
            new Dictionary<Type, object>()
            {
                {typeof (ICreateEntity), CreateAction},
                {typeof (IDeleteEntity), DeleteAction},
                {typeof (IUpdateEntityWithChanges), UpdateAction},
                {typeof (IGetEntityById), GetEntityByIdAction},
                {typeof (IGetEntityWithChanges), GetEntityWithChangesAction},

                {typeof (ILoadEntitySet), LoadEntitySet},
                {typeof (IGetEntitySetWithChanges), LoadEntitySetWithChanges},
                {typeof (ILoadEntitySetWithFilter), LoadEntitySetWithFilter},
                {typeof (ILoadEntitySetWithFilterWithIncludes), LoadEntitySetWithFilterWithIncludes},
                
            };
        
        public EntityDataServiceSupervisor(IDynamicEntityType entityType, ISystemProcess process, IProcessSystemMessage msg) : base(process)
        {
            EntityType = entityType;

            foreach (var itm in entityEvents)
            {
                Task.Run(() => {this.GetType()
                        .GetMethod("CreateEntityActor")
                        .MakeGenericMethod(itm.Key)
                        .Invoke(this, new object[] {itm.Value,entityType, process, msg}); }).ConfigureAwait(false); 
            }
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceSupervisor>(this, new StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, RevolutionData.Context.Actor.Events.ActorStarted)), process, Source));

            
        }

        public void CreateEntityActor<TEvent>(object action,IDynamicEntityType entityType, ISystemProcess process, IProcessSystemMessage msg) where TEvent : class, IEntityRequest
        {
            /// Create Actor Per Event
            
           
                
                    Type actorType = typeof(EntityDataServiceActor<>).MakeGenericType(typeof(TEvent));
                    var inMsg = new CreateEntityService(actorType.GetFriendlyName(),actorType, action,(IEntityRequest)msg, new StateCommandInfo(process, RevolutionData.Context.CommandFunctions.UpdateCommandData(typeof(TEvent).GetFriendlyName(), RevolutionData.Context.Actor.Commands.StartActor)), process, Source);
                    try
                    {

                        var child = Activator.CreateInstance(actorType, inMsg, entityType);

                    }
                    catch (Exception ex)
                    {
                        //ToDo: This seems like a good way... getting the expected event type 
                        PublishProcesError(inMsg, ex, inMsg.ProcessInfo.State.ExpectedEvent.GetType());
                    }

                
            
        }

        public IDynamicEntityType EntityType { get; }
    }

}