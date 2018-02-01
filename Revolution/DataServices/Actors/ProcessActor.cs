using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;
using IProcessService = Actor.Interfaces.IProcessService;

namespace DataServices.Actors
{


    public class ProcessActor : BaseActor<ProcessActor>, IProcessService
    {

        private ConcurrentQueue<IProcessSystemMessage> msgQue = new ConcurrentQueue<IProcessSystemMessage>();
        private ConcurrentQueue<IComplexEventAction> _complexEvents = new ConcurrentQueue<IComplexEventAction>();

        public ConcurrentDictionary<Type, IProcessStateMessage> ProcessStateMessages { get; } =
            new ConcurrentDictionary<Type, IProcessStateMessage>();

        private static IUntypedActorContext ctx = null;

        private string ActorName = null;

        public ProcessActor(ICreateProcessActor msg) : base(msg.Process)
        {
            ActorName = msg.ActorName;
            ctx = Context;
            EventMessageBus.Current.GetEvent<IProcessStateMessage>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(x => SaveStateMessages(x));
            EventMessageBus.Current.GetEvent<IRequestProcessState>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(x => HandleRequestState(x));
            EventMessageBus.Current.GetEvent<IRequestProcessLog>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(x => HandleProcessLogRequest(x));
            EventMessageBus.Current.GetEvent<IComplexEventLogCreated>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(x => HandleComplexEventLog(x));

            EventMessageBus.Current.GetEvent<IServiceStarted<IComplexEventService>>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(x => NotifyServiceStarted(x));

            EventMessageBus.Current.GetEvent<ICreateProcessActor>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Where(x => x.ActorName == this.ActorName).Subscribe(x => UpdateActor(x.ComplexEvents));

            EventMessageBus.Current.GetEvent<ICleanUpSystemProcess>(Source)
                .Where(x => x.ProcessToBeCleanedUp.Id > 1 && x.ProcessToBeCleanedUp.Id == Process.Id)
                .Subscribe(x => CleanUpActor(x));

            EventMessageBus.Current.GetEvent<IServiceStarted<IProcessService>>(Source)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Subscribe(q =>
                {
                    EventMessageBus.Current.GetEvent<IProcessSystemMessage>(Source)
                        .Where(
                            x =>
                                x.Process.Id == Process.Id &&
                                x.MachineInfo.MachineName == Process.MachineInfo.MachineName)
                        .Subscribe(z => HandleProcessEvents(z));

                });

            Parallel.ForEach(msg.ComplexEvents, itm => { _complexEvents.Enqueue(itm); });
            StartActors(_complexEvents);
            EventMessageBus.Current.GetEvent<ILoadProcessComplexEvents>(Source)
                .Where(x => $"{x.Name}".GetSafeActorName() == ActorName).Subscribe(x => HandleDomainProcess(x));

        }

        private void CleanUpActor(ICleanUpSystemProcess cleanUpSystemProcess)
        {
            _complexEvents = new ConcurrentQueue<IComplexEventAction>();
        }


        private void UpdateActor(IList<IComplexEventAction> complexEvents)
        {
            var lst = new List<IComplexEventAction>(_complexEvents);
            var newLst = complexEvents.Where(x => _complexEvents.All(z => z.Key != x.Key)).ToList();
            lst.AddRange(newLst);
            Parallel.ForEach(lst, itm => { _complexEvents.Enqueue(itm); });
            StartActors(newLst);
        }

        private void HandleDomainProcess(ILoadProcessComplexEvents loadProcessComplexEvents)
        {

            UpdateActor(loadProcessComplexEvents.ComplexEvents);
            Publish(new ServiceStarted<IProcessService>(this,
                new StateEventInfo(Process, RevolutionData.Context.Actor.Events.ActorStarted), Process, Source));
            
        }

        ConcurrentQueue<IServiceStarted<IComplexEventService>> startedComplexEventServices =
            new ConcurrentQueue<IServiceStarted<IComplexEventService>>();

        private void NotifyServiceStarted(IServiceStarted<IComplexEventService> service)
        {
            startedComplexEventServices.Enqueue(service);
            if (startedComplexEventServices.Count != _complexEvents.Count()) return;
            Publish(new ServiceStarted<IProcessService>(this,
                new StateEventInfo(Process, RevolutionData.Context.Actor.Events.ActorStarted), Process, Source));

        }


        ConcurrentQueue<IComplexEventLogCreated> complexEventLogs = new ConcurrentQueue<IComplexEventLogCreated>();

        private void HandleComplexEventLog(IComplexEventLogCreated complexEventLog)
        {
            complexEventLogs.Enqueue(complexEventLog);
            if (complexEventLogs.Count != _complexEvents.Count()) return;

            var msg = CreateProcessLog();
            Publish(msg);
            complexEventLogs = new ConcurrentQueue<IComplexEventLogCreated>();
        }

        private ProcessLogCreated CreateProcessLog()
        {
            var logs = new List<IComplexEventLog>(msgQue.ToImmutableList().CreatEventLogs(Source));

            var msg = new ProcessLogCreated(logs.OrderBy(x => x.Time),
                new StateEventInfo(Process, RevolutionData.Context.Process.Events.LogCreated), Process, Source);
            return msg;
        }

        private void HandleProcessLogRequest(IRequestProcessLog requestProcessLog)
        {
            //Request logs from ComplexEventActors

            var msg = CreateProcessLog();
            Publish(msg);
        }

        private void HandleRequestState(IRequestProcessState requestProcessState)
        {
            foreach (var ps in ProcessStateMessages)
            {
                Publish(ps.Value);
            }
        }

        private void SaveStateMessages(IProcessStateMessage pe)
        {

            ProcessStateMessages.AddOrUpdate(pe.GetType(), pe, (k, v) => pe);
            var msg = new ProcessStateUpdated(pe.EntityType, pe,
                new StateEventInfo(Process, RevolutionData.Context.Process.Events.StateUpdated), Process, Source);
            Publish(msg);
        }

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

        private void HandleProcessEvents(IProcessSystemMessage pe)
        {
            // Log the message 
            //TODO: Reenable event log
            //Persist(pe, x => { });//(x) => msgQue.Add(x)

            // send out Process State Events

            msgQue.Enqueue(pe);

        }


        public IActorRef ActorRef => this.Self;

        private Action<ICreateComplexEventService> CreateComplexEventService = inMsg =>
        {
            try
            {
                Task.Run(() =>
                {
                   ctx.ActorOf(Props.Create<ComplexEventActor>(inMsg),
                        $"ComplexEventActor:-{inMsg.ComplexEventService.ActorId.GetSafeActorName()}-{inMsg.Process.Id}");
                }).ConfigureAwait(false);

            }
            catch (Exception)
            {

                throw;
            }


        };
    }


}