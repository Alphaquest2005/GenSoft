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

        private static readonly Action<ISystemSource, ICreateEntity> CreateAction = (s, x) => x.CreateEntity();
        private static readonly Action<ISystemSource, IDeleteEntity> DeleteAction = (s, x) => x.DeleteEntity();
        private static readonly Action<ISystemSource, IUpdateEntityWithChanges> UpdateAction = (s, x) => x.UpdateEntity();
        private static readonly Action<ISystemSource, IAddOrGetEntityWithChanges> AddAction = (s, x) => x.AddEntity();
        private static readonly Action<ISystemSource, IGetEntityById> GetEntityByIdAction = (s, x) => x.GetEntity();
        private static readonly Action<ISystemSource, IGetEntityWithChanges> GetEntityWithChangesAction = (s, x) => x.GetEntity();

        private static readonly Action<ISystemSource, ILoadEntitySet> LoadEntitySet = (s, x) => x.LoadEntitySet();
        private static readonly Action<ISystemSource, IGetEntitySetWithChanges> LoadEntitySetWithChanges = (s, x) => x.LoadEntitySetWithChanges();
        private static readonly Action<ISystemSource, ILoadEntitySetWithFilter> LoadEntitySetWithFilter = (s, x) => x.LoadEntitySet();
        private static readonly Action<ISystemSource, ILoadEntitySetWithFilterWithIncludes> LoadEntitySetWithFilterWithIncludes = (s, x) => x.LoadEntitySet();

        

        readonly Dictionary<Type, object> entityEvents =
            new Dictionary<Type, object>()
            {
                {typeof (ICreateEntity), CreateAction},
                {typeof (IDeleteEntity), DeleteAction},
                {typeof (IUpdateEntityWithChanges), UpdateAction},
                {typeof (IAddOrGetEntityWithChanges), AddAction},
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
            EventMessageBus.Current.Publish(new ServiceStarted<IEntityDataServiceSupervisor>(this, new StateEventInfo(process, RevolutionData.Context.EventFunctions.UpdateEventData(entityType.Name, RevolutionData.Context.Actor.Events.ActorStarted)), process, Source), Source);

            
        }

        public void CreateEntityActor<TEvent>(object action,IDynamicEntityType entityType, ISystemProcess process, IProcessSystemMessage msg) where TEvent : class, IEntityRequest
        {
            /// Create Actor Per Event
            
           
                
                    Type actorType = typeof(EntityDataServiceActor<>).MakeGenericType(typeof(TEvent));
                    var inMsg = new CreateEntityService(actorType.GetFriendlyName(),actorType, action, new StateCommandInfo(process, RevolutionData.Context.CommandFunctions.UpdateCommandData(typeof(TEvent).GetFriendlyName(), RevolutionData.Context.Actor.Commands.StartActor)), process, Source);
                    try
                    {

                        var child = Activator.CreateInstance(actorType, inMsg, entityType, msg);

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