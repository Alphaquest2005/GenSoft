using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;

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

       
        public ProcessActor(ICreateProcessActor msg) : base(msg.ActorId, msg.Process)
        {
            
            
          
            //EventMessageBus.Current.GetEvent<IRequestProcessLog>(Source)
            //    .Where(x => x.Process.Id == msg.Process.Id)
            //    .Subscribe(x => HandleProcessLogRequest(x));

            //EventMessageBus.Current.GetEvent<IComplexEventLogCreated>(Source)
            //    .Where(x => x.Process.Id == msg.Process.Id)
            //    .Subscribe(x => HandleComplexEventLog(x));

            Publish(new ServiceStarted<IProcessService>(this,
                new StateEventInfo(Process,RevolutionData.Context.EventFunctions.UpdateEventData(ActorId,RevolutionData.Context.Actor.Events.ActorStarted)), Process, Source));

            EventMessageBus.Current.GetEvent<ICreateProcessActor>(msg.ProcessInfo,Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == msg.ProcessInfo.EventKey)
                .Where(x => x.Process.Id == msg.Process.Id)
                .Where(x => x.ActorId == this.ActorId).Subscribe(x => UpdateActor(x));

            var stateCommandInfo = new StateCommandInfo(msg.Process, RevolutionData.Context.Process.Commands.CleanUpProcess, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<ICleanUpSystemProcess>( stateCommandInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == stateCommandInfo.EventKey)
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

            
            StartActors(msg);

            var processStateInfo = new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData(ActorId, RevolutionData.Context.Process.Commands.StartProcess), Guid.NewGuid());
            EventMessageBus.Current.GetEvent<ILoadProcessComplexEvents>(processStateInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == processStateInfo.EventKey)
                .Where(x => $"{x.Name}".GetSafeActorName() == ActorId)
                .Subscribe(x => HandleDomainProcess(x));

        }

        private void CleanUpActor(ICleanUpSystemProcess cleanUpSystemProcess)
        {
            //Self.Tell(PoisonPill.Instance);
        }


        private void UpdateActor(ICreateProcessActor complexEvents)
        {
            StartActors(complexEvents);
        }

        private void HandleDomainProcess(ILoadProcessComplexEvents loadProcessComplexEvents)
        {
            try
            {
                CreateComplexEventActors(loadProcessComplexEvents.ComplexEvents);
            }
            catch (Exception ex)
            {
                PublishProcesError(loadProcessComplexEvents, ex, loadProcessComplexEvents.GetType());
            }
            
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


        private void StartActors(ICreateProcessActor msg)
        {
            Contract.Requires(msg.ComplexEvents.Any() && msg.ComplexEvents != null);
            //Parallel.ForEach(complexEvents, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
            //    (cp) =>
            //    {
            try
            {
                CreateComplexEventActors(msg.ComplexEvents);
            }
            catch (Exception ex)
            {
                PublishProcesError(msg, ex, msg.GetType());
            }

            //});
        }

        private void CreateComplexEventActors(IReadOnlyList<IComplexEventAction> complexEvents)
        {
            foreach (var cp in complexEvents)
            {
                var inMsg = new CreateComplexEventService(new ComplexEventService(cp.Key, cp, Process, Source),
                    new StateCommandInfo(Process,
                        RevolutionData.Context.CommandFunctions.UpdateCommandData(cp.Key,
                            RevolutionData.Context.Actor.Commands.StartActor), Guid.NewGuid()), Process,
                    Source);
                
                try
                {
                    CreateComplexEventService(inMsg);
                }
                catch (Exception ex)
                {
                    PublishProcesError(inMsg, ex, typeof(IServiceStarted<IComplexEventService>));
                }

                //Publish(inMsg);
            }
        }




        private void CreateComplexEventService(ICreateComplexEventService inMsg)
        {
            try
            {

                

                    try
                    {
                        var cp = new ComplexEventActor(inMsg);
                    }
                    catch (Exception ex)
                    {
                        PublishProcesError(inMsg, ex, inMsg.GetType());
                    }
                
            }
            catch (Exception)
            {

                throw;
            }


        }
    }


}