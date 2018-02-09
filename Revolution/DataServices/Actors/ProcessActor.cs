using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using DataServices.Utils;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionEntities.Process;
using Utilities;
using IProcessService = Actor.Interfaces.IProcessService;

namespace DataServices.Actors
{


    public class ProcessActor : BaseActor<ProcessActor>, IProcessService
    {

        //private ConcurrentQueue<IProcessSystemMessage> msgQue = new ConcurrentQueue<IProcessSystemMessage>();
        
        //public ConcurrentDictionary<Type, IProcessStateMessage> ProcessStateMessages { get; } = new ConcurrentDictionary<Type, IProcessStateMessage>();

        private static IUntypedActorContext ctx = null;

        private string ActorName = null;

        public ProcessActor(ICreateProcessActor msg) : base(msg.Process)
        {
            ActorName = msg.ActorName;
            ctx = Context;
          
            //EventMessageBus.Current.GetEvent<IRequestProcessLog>(Source)
            //    .Where(x => x.Process.Id == msg.Process.Id)
            //    .Subscribe(x => HandleProcessLogRequest(x));

            //EventMessageBus.Current.GetEvent<IComplexEventLogCreated>(Source)
            //    .Where(x => x.Process.Id == msg.Process.Id)
            //    .Subscribe(x => HandleComplexEventLog(x));

            Publish(new ServiceStarted<IProcessService>(this,
                new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventStatus(ActorName,RevolutionData.Context.Actor.Events.ActorStarted)), Process, Source));

            EventMessageBus.Current.GetEvent<ICreateProcessActor>(msg.ProcessInfo,Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Where(x => x.ActorName == this.ActorName).Subscribe(x => UpdateActor(x.ComplexEvents));

            EventMessageBus.Current.GetEvent<ICleanUpSystemProcess>( new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CleanUpProcess), Source)
                .Where(x => x.ProcessToBeCleanedUp.Id > 1 && x.ProcessToBeCleanedUp.Id == Process.Id)
                .Subscribe(x => CleanUpActor(x));

            //EventMessageBus.Current.GetEvent<IServiceStarted<IProcessService>>(Source)
            //    .Where(x => x.Process.Id == msg.Process.Id)
            //    .Subscribe(q =>
            //    {
            //        EventMessageBus.Current.GetEvent<IProcessSystemMessage>(Source)
            //            .Where(
            //                x =>
            //                    x.Process.Id == Process.Id &&
            //                    x.MachineInfo.MachineName == Process.MachineInfo.MachineName)
            //            .Subscribe(z => HandleProcessEvents(z));

            //    });

            
            StartActors(msg.ComplexEvents);
            EventMessageBus.Current.GetEvent<ILoadProcessComplexEvents>(new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandStatus(ActorName, RevolutionData.Context.Process.Commands.StartProcess)), Source)
                .Where(x => $"{x.Name}".GetSafeActorName() == ActorName).Subscribe(x => HandleDomainProcess(x));

        }

        private void CleanUpActor(ICleanUpSystemProcess cleanUpSystemProcess)
        {
           Self.Tell(PoisonPill.Instance);
        }


        private void UpdateActor(IList<IComplexEventAction> complexEvents)
        {
            StartActors(complexEvents);
        }

        private void HandleDomainProcess(ILoadProcessComplexEvents loadProcessComplexEvents)
        {
            UpdateActor(loadProcessComplexEvents.ComplexEvents);
            Publish(new ServiceStarted<IProcessService>(this,
                new StateEventInfo(Process, RevolutionData.Context.Actor.Events.ActorStarted), Process, Source));
        }

        //ConcurrentQueue<IServiceStarted<IComplexEventService>> startedComplexEventServices =
        //    new ConcurrentQueue<IServiceStarted<IComplexEventService>>();

       

        //ConcurrentQueue<IComplexEventLogCreated> complexEventLogs = new ConcurrentQueue<IComplexEventLogCreated>();

        //private void HandleComplexEventLog(IComplexEventLogCreated complexEventLog)
        //{
        //    complexEventLogs.Enqueue(complexEventLog);
           
        //    var msg = CreateProcessLog();
        //    Publish(msg);
        //    complexEventLogs = new ConcurrentQueue<IComplexEventLogCreated>();
        //}


        //private void HandleProcessLogRequest(IRequestProcessLog requestProcessLog)
        //{
        //    //Request logs from ComplexEventActors

        //    var msg = CreateProcessLog();
        //    Publish(msg);
        //}

     
        private void StartActors(IEnumerable<IComplexEventAction> complexEvents)
        {
            Contract.Requires(complexEvents.Any() && complexEvents != null);
            Parallel.ForEach(complexEvents, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (cp) =>
                {
                    //foreach (var cp in complexEvents)
                    //{
                    var inMsg = new CreateComplexEventService(new ComplexEventService(cp.Key, cp, Process, Source),
                        new StateCommandInfo(Process, RevolutionData.Context.Actor.Commands.StartActor), Process,
                        Source);


                    Publish(inMsg);
                    try
                    {
                        CreateComplexEventService.Invoke(inMsg);
                    }
                    catch (Exception ex)
                    {
                        PublishProcesError(inMsg, ex, typeof(IServiceStarted<IComplexEventService>));
                    }
                    // }
                });
        }


        public IActorRef ActorRef => this.Self;

        private Action<ICreateComplexEventService> CreateComplexEventService = inMsg =>
        {
            try
            {

                Task.Run(() =>
                {
                //var child = ctx.Child(
                //    $"ComplexEventActor:-{inMsg.ComplexEventService.ActorId.GetSafeActorName()}-{inMsg.Process.Id}");
                //if (Equals(child, ActorRefs.Nobody))
                //{
                    try
                        {
                            ctx.ActorOf(Props.Create<ComplexEventActor>(inMsg),
                                $"ComplexEventActor:-{inMsg.ComplexEventService.ActorId.GetSafeActorName()}-{inMsg.Process.Id}");
                        }
                        catch (Exception ex)
                        {
                           // if (!ex.Message.Contains("is not unique!")) throw;
                        }

                   //}
                }).ConfigureAwait(false);

            }
            catch (Exception)
            {

                throw;
            }


        };
    }


}