using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemInterfaces;
using Akka.Actor;
using Akka.IO;
using Akka.Routing;
using CommonMessages;
using EventAggregator;
using EventMessages;
using EventMessages.Commands;
using RevolutionEntities.Process;
using Utilities;
using ViewMessages;

namespace DataServices.Actors
{
    public class EntityDataServiceSupervisor : BaseSupervisor<EntityDataServiceSupervisor> 
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
                { typeof (IGetEntitySetWithChanges), LoadEntitySetWithChanges},
                {typeof (ILoadEntitySetWithFilter), LoadEntitySetWithFilter},
                {typeof (ILoadEntitySetWithFilterWithIncludes), LoadEntitySetWithFilterWithIncludes},
                
            };
        private IUntypedActorContext ctx = null;
        public EntityDataServiceSupervisor(ISystemProcess process, IProcessSystemMessage msg) : base(process)
        {
            ctx = Context;
            foreach (var itm in entityEvents)
            {
              this.GetType()
                        .GetMethod("CreateEntityActor")
                        .MakeGenericMethod(itm.Key)
                        .Invoke(this, new object[] {itm.Value, process, msg});
            }

        }

        public void CreateEntityActor<TEvent>(object action, ISystemProcess process, IProcessSystemMessage msg) where TEvent : IMessage
        {
            /// Create Actor Per Event
            Type actorType = typeof(EntityDataServiceActor<>).MakeGenericType(typeof(TEvent));
            var inMsg = new CreateEntityService(actorType,action, new StateCommandInfo(process.Id, RevolutionData.Context.Actor.Commands.StartActor),process,Source );
            try
            {
                
                Task.Run(() =>
                {
                    ctx.ActorOf(
                        Props.Create(actorType, inMsg, msg)
                            .WithRouter(new RoundRobinPool(1,
                                new DefaultResizer(1, Environment.ProcessorCount, 1, .2, .3, .1,
                                    Environment.ProcessorCount))),
                        "EntityDataServiceActor-" +
                        typeof (TEvent).GetFriendlyName().Replace("<", "'").Replace(">", "'"));
                });
                

            }
            catch (Exception ex)
            {
                //ToDo: This seems like a good way... getting the expected event type 
                PublishProcesError(inMsg, ex, inMsg.ProcessInfo.State.ExpectedEvent.GetType());
            }
            
        }

       

      

    }

}