using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using EventAggregator;
using RevolutionEntities.Process;
using DataServices.Utils;
using EventMessages.Commands;
using EventMessages.Events;
using Utilities;


namespace DataServices.Actors
{


    public class ComplexEventActor : BaseActor<ComplexEventActor>, IComplexEventService
    {
        public ComplexEventActor(ICreateComplexEventService msg) : base(msg.ComplexEventService.ActorId,msg.Process)
        {
            try
            {


                ComplexEventAction = msg.ComplexEventService.ComplexEventAction;
                //Process = msg.ComplexEventService.Process;
                
                foreach (var e in msg.ComplexEventService.ComplexEventAction.Events)
                {
                    this.GetType().GetMethod("WireEvents").MakeGenericMethod(e.EventType)
                        .Invoke(this, new object[] {e});
                }
                //Todo: make time out configurable

                var cleanUpSystemProcessCommandInfo = new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CleanUpProcess, Guid.NewGuid());
                EventMessageBus.Current
                    .GetEvent<ICleanUpSystemProcess>(cleanUpSystemProcessCommandInfo,Source)
                    .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == cleanUpSystemProcessCommandInfo.EventKey)
                    .Where(x => x.ProcessToBeCleanedUp.Id > 1 && x.ProcessToBeCleanedUp.Id == Process.Id)
                    .Subscribe(x => CleanUpActor(x));

                var requestComplexEventLogCommandInfo = new StateCommandInfo(msg.Process,
                    RevolutionData.Context.Process.Commands.CreateComplexEventLog, Guid.NewGuid());
                EventMessageBus.Current.GetEvent<IRequestComplexEventLog>(requestComplexEventLogCommandInfo, Source)
                    .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == requestComplexEventLogCommandInfo.EventKey)
                    .Subscribe(x => handleComplexEventLogRequest());

                Publish(new ServiceStarted<IComplexEventService>(this,
                    new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventData(msg.ComplexEventService.ComplexEventAction.Key,RevolutionData.Context.Actor.Events.ActorStarted)), Process, Source));
            }
            catch (Exception ex)
            {
                PublishProcesError(msg, ex, msg.GetType());
            }
        }

        private void CleanUpActor(ICleanUpSystemProcess cleanUpSystemProcess)
        {
            
            ComplexEventAction = new ComplexEventAction();
            InMessages.Clear();
            
        }

        private void handleComplexEventLogRequest()
        {
            var xlogs = ComplexEventAction.Events.CreatEventLogs(InMessages.ToDictionary(x => x.Key, x => x.Value), Source);
            var ologs = OutMessages.CreatEventLogs(Source);
            var res = new List<IComplexEventLog>();
            res.AddRange(xlogs);
            res.AddRange(ologs);

            var msg = new ComplexEventLogCreated(res, new StateEventInfo(Process, RevolutionData.Context.Process.Events.ComplexEventLogCreated), Process, Source);
            Publish(msg);

        }

        private void OnTimeOut()
        {
            //if (ComplexEventAction.Events.All(z => z.Raised())) return;
            if (InMessages.Count == ComplexEventAction.Events.Count) return;
            //Create Timeout Message
            var timeoutMsg = new ComplexEventActionTimedOut(ComplexEventAction, new StateEventInfo(Process, RevolutionData.Context.Process.Events.ProcessTimeOut), Process, Source);
            PublishProcesError(timeoutMsg, new ApplicationException($"ComplexEventActionTimedOut:<{ComplexEventAction.ProcessInfo.State.Name}>"), ComplexEventAction.ExpectedMessageType);

        }



        public void WireEvents<TEvent>(IProcessExpectedEvent expectedEvent) where TEvent :class, IProcessSystemMessage
        {
            
            EventMessageBus.Current.GetEvent<TEvent>(expectedEvent.ProcessInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == expectedEvent.ProcessInfo.EventKey)
                .Where(x => x.Process.Id == Process.Id)
                //.Where(x => x.GetType().GetInterfaces().Any(z => z == expectedEvent.EventType))
                .Subscribe( x => CheckEvent(expectedEvent, x));
        }

        private void CheckEvent(IProcessExpectedEvent expectedEvent, IProcessSystemMessage message)
        {
            //todo: good implimentation of Railway pattern chain execution with error handling
            if (!expectedEvent.EventPredicate.Invoke(message)) return;
            expectedEvent.Validate(message);
            InMessages.AddOrUpdate(expectedEvent.Key, message, (k, v) => message);
            if (ComplexEventAction.ActionTrigger != ActionTrigger.Any && InMessages.Count() != ComplexEventAction.Events?.Count) return;

           
                ExecuteAction(InMessages.ToImmutableDictionary(x => x.Key, x => x.Value.Message));
           

            //if (ComplexEventAction.ActionTrigger == ActionTrigger.All)
            //{
                InMessages.Clear();
            //}
            //else
            //{
            //    IProcessSystemMessage msg;
            //    InMessages.TryRemove(expectedEvent.Key, out msg);
            //}

        }

        private void ExecuteAction(ImmutableDictionary<string, IDynamicObject> msgs)
        {
            // if (!ComplexEventAction.Events.All(z => z.Raised())) return;
            
            var inMsg = new ExecuteComplexEventAction(ComplexEventAction.Action,
                new DynamicComplexEventParameters(this, msgs),
                new StateCommandInfo(Process,
                    RevolutionData.Context.CommandFunctions.UpdateCommandData(ComplexEventAction.Key,
                        RevolutionData.Context.Actor.Commands.CreateAction)), Process, Source);
            
            var outMsg = ComplexEventAction.Action.Action(inMsg.ComplexEventParameters).Result;
            try
            {
                Publish(outMsg);

            }
            catch (Exception ex)
            {
                PublishProcesError(inMsg, ex, ComplexEventAction.ExpectedMessageType);
            }

            Publish(inMsg);

        }



        public IComplexEventAction ComplexEventAction { get; private set; }
        //public ISystemProcess Process { get; }
        private readonly ConcurrentDictionary<string, IProcessSystemMessage> InMessages = new ConcurrentDictionary<string, IProcessSystemMessage>();

    }


}