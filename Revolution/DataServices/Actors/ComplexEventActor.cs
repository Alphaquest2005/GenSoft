﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
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
        public ComplexEventActor(ICreateComplexEventService msg) : base(msg.Process)
        {
            ComplexEventAction = msg.ComplexEventService.ComplexEventAction;
            //Process = msg.ComplexEventService.Process;
            ActorId = msg.ComplexEventService.ActorId;
            foreach (var e in msg.ComplexEventService.ComplexEventAction.Events)
            {
                this.GetType().GetMethod("WireEvents").MakeGenericMethod(e.EventType).Invoke(this, new object[] { e });
            }
            //Todo: make time out configurable

            EventMessageBus.Current.GetEvent<ICleanUpSystemProcess>(new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CleanUpProcess), Source).Where(x => x.ProcessToBeCleanedUp.Id > 1 && x.ProcessToBeCleanedUp.Id == Process.Id).Subscribe(x => CleanUpActor(x));
            EventMessageBus.Current.GetEvent<IRequestComplexEventLog>(new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CreateComplexEventLog), Source).Subscribe(x => handleComplexEventLogRequest());
            
            Publish(new ServiceStarted<IComplexEventService>(this, new StateEventInfo(Process, RevolutionData.Context.EventFunctions.UpdateEventStatus(msg.ComplexEventService.ComplexEventAction.ExpectedMessageType.GetFriendlyName(), RevolutionData.Context.Actor.Events.ActorStarted)), Process, Source));
        }

        private void CleanUpActor(ICleanUpSystemProcess cleanUpSystemProcess)
        {
            
            ComplexEventAction = new ComplexEventAction();
            InMessages.Clear();
            Self.Tell(PoisonPill.Instance);
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
                .Where(x => x.Process.Id == Process.Id)
                .Where(x => x.GetType().GetInterfaces().Any(z => z == expectedEvent.EventType)).Subscribe(async x => await CheckEvent(expectedEvent, x).ConfigureAwait(false));
        }

        private async Task CheckEvent(IProcessExpectedEvent expectedEvent, IProcessSystemMessage message)
        {
            //todo: good implimentation of Railway pattern chain execution with error handling
            if (!expectedEvent.EventPredicate.Invoke(message)) return;
            expectedEvent.Validate(message);
            InMessages.AddOrUpdate(expectedEvent.Key, message, (k, v) => message);
            if (ComplexEventAction.ActionTrigger != ActionTrigger.Any && InMessages.Count() != ComplexEventAction.Events?.Count) return;
            await ExecuteAction(InMessages.ToImmutableDictionary(x => x.Key, x => x.Value.Message)).ConfigureAwait(false);

            if (ComplexEventAction.ActionTrigger == ActionTrigger.All)
            {
                InMessages.Clear();
            }
            else
            {
                IProcessSystemMessage msg;
                InMessages.TryRemove(expectedEvent.Key, out msg);
            }

        }

        private async Task ExecuteAction(ImmutableDictionary<string, IDynamicObject> msgs)
        {
            // if (!ComplexEventAction.Events.All(z => z.Raised())) return;
            if (ComplexEventAction.Action == null) return;
            var inMsg = new ExecuteComplexEventAction(ComplexEventAction.Action, new DynamicComplexEventParameters(this,  msgs), new StateCommandInfo(Process, RevolutionData.Context.CommandFunctions.UpdateCommandStatus(ComplexEventAction.Key, RevolutionData.Context.Actor.Commands.CreateAction)), Process, Source);

            Publish(inMsg);
            var outMsg = await ComplexEventAction.Action.Action(inMsg.ComplexEventParameters).ConfigureAwait(false);
            try
            {
                Publish(outMsg);

            }
            catch (Exception ex)
            {
                PublishProcesError(inMsg, ex, ComplexEventAction.ExpectedMessageType);
            }

        }


        public string ActorId { get; }
        public IComplexEventAction ComplexEventAction { get; private set; }
        //public ISystemProcess Process { get; }
        private readonly ConcurrentDictionary<string, IProcessSystemMessage> InMessages = new ConcurrentDictionary<string, IProcessSystemMessage>();

    }


}