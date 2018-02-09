using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities.Process;
using Utilities;


namespace DataServices.Actors
{
    public class SystemProcessSupervisor : BaseSupervisor<SystemProcessSupervisor>
    {
        private IUntypedActorContext ctx = null;

        //TODO: Track Actor Shutdown instead of just broadcast

        public SystemProcessSupervisor(ISystemStarted firstMsg, List<IComplexEventAction> processComplexEvents) : base(
            firstMsg.Process)
        {

            ctx = Context;

            EventMessageBus.Current.GetEvent<ILoadProcessComplexEvents>(new StateEventInfo(firstMsg.Process, RevolutionData.Context.Process.Events.ProcessStarted), Source).Subscribe(HandleDomainProcess);
            StartProcess(processComplexEvents, firstMsg.User);
        }

        private void HandleDomainProcess(ILoadProcessComplexEvents loadProcessComplexEvents)
        {
            StartProcess(loadProcessComplexEvents.ComplexEvents, loadProcessComplexEvents.User);
        }

        private void StartProcess(List<IComplexEventAction> complexEventActions, IUser user)
        {
            CreateProcesses(user, complexEventActions);
        }



        private void CreateProcesses(IUser user, List<IComplexEventAction> complexEventActions)
        {
            var c = new CreateProcessActor(
                $"{complexEventActions.First().Process.Name.GetSafeActorName()}:{complexEventActions.First().Process.Id}",
                complexEventActions,
                new StateCommandInfo(complexEventActions.First().Process,
                    RevolutionData.Context.CommandFunctions.UpdateCommandStatus(complexEventActions.First().Key, RevolutionData.Context.Actor.Commands.CreateActor)),
                complexEventActions.First().Process, Source);
            PublishActor(c);
        }

        private void PublishActor(CreateProcessActor inMsg)
        {
            try
            {
                // var actorName = "ProcessActor-" + inMsg.Process.Name.GetSafeActorName();



                Task.Run(() =>
                    {
                        //var child = ctx.Child(inMsg.ActorName);
                        //if (Equals(child, ActorRefs.Nobody))
                        //{
                        try
                        {
                            ctx.ActorOf(Props.Create<ProcessActor>(inMsg), inMsg.ActorName);
                        }
                        catch (Exception e)
                        {
                          
                        }
                            
                        //}
                    }).ConfigureAwait(false);



                EventMessageBus.Current.Publish(inMsg, Source);


            }
            catch (Exception ex)
            {
                Debugger.Break();
                EventMessageBus.Current.Publish(new ProcessEventFailure(failedEventType: inMsg.GetType(),
                        failedEventMessage: inMsg,
                        expectedEventType: typeof(SystemProcessStarted),
                        exception: ex,
                        source: Source,
                        processInfo:
                        new StateEventInfo(inMsg.Process, RevolutionData.Context.Process.Events.Error)),
                    Source);
            }
        }


    }


}