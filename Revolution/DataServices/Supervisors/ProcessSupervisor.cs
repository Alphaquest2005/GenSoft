using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;


namespace DataServices.Actors
{
    public class SystemProcessSupervisor : BaseSupervisor<SystemProcessSupervisor>
    {
        private IUntypedActorContext ctx = null;

        //TODO: Track Actor Shutdown instead of just broadcast

        public SystemProcessSupervisor(bool autoRun, ISystemStarted firstMsg, List<ISystemProcessInfo> processInfos, List<IComplexEventAction> processComplexEvents) : base(firstMsg.Process)
        {

            ctx = Context;
            ProcessInfos = processInfos;
            ProcessComplexEvents = processComplexEvents;
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(Source).Where(x => autoRun && x.ProcessToBeStartedId == ProcessActions.NullProcess).Subscribe(x => StartParentProcess(x.Process.Id, x.User));
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(Source).Where(x => !autoRun && x.ProcessToBeStartedId != ProcessActions.NullProcess).Subscribe(x => StartProcess(x.ProcessToBeStartedId,x.User));
            EventMessageBus.Current.GetEvent<ILoadDomainProcess>(Source).Subscribe(x => HandleDomainProcess(x));
            StartProcess(firstMsg.Process.Id,firstMsg.User);
        }

        private void HandleDomainProcess(ILoadDomainProcess loadDomainProcess)
        {
            ProcessComplexEvents.AddRange(loadDomainProcess.ComplexEvents);
            if(ProcessInfos.All(x => x.Id != loadDomainProcess.Process.Id))  ProcessInfos.Add(new SystemProcessInfo(loadDomainProcess.Process.Id, loadDomainProcess.Process.ParentProcessId, loadDomainProcess.Process.Name, loadDomainProcess.Process.Description, loadDomainProcess.Process.Symbol, loadDomainProcess.Process.User.UserId));
            StartProcess(loadDomainProcess.Process.Id, loadDomainProcess.User);
        }


        private void StartParentProcess(int processId, IUser user)
        {
            var processSteps = ServiceManager.ProcessInfos.Where(x => x.ParentProcessId == processId);
            CreateProcesses(user, processSteps, processSteps.First().Id);
        }

        public List<ISystemProcessInfo> ProcessInfos { get; }

        private void StartProcess(int processId, IUser user)
        {
            var processSteps = ProcessInfos.Where(x => x.Id == processId);
            CreateProcesses(user, processSteps, processId);
        }

        //static ConcurrentDictionary<string,string> existingProcessActors = new ConcurrentDictionary<string, string>();

        private void CreateProcesses(IUser user, IEnumerable<ISystemProcessInfo> processSteps, int processId)
        {
           
            if (ProcessComplexEvents.All(x => x.ProcessId != processId)) return;
            //Parallel.ForEach(
            //    processSteps.Select(
            //        p =>
            //            new CreateProcessActor($"{p.Name.GetSafeActorName()}-{p.Id}", ProcessComplexEvents.Where(x => x.ProcessId == processId).ToList(),
            //                new StateCommandInfo(p.Id, RevolutionData.Context.Actor.Commands.CreateActor),
            //                new SystemProcess(
            //                    new RevolutionEntities.Process.Process(p.Id, p.ParentProcessId, p.Name, p.Description, p.Symbol, user),
            //                    Source.MachineInfo), Source)), new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
            //    PublishActor);

            var cpLst = ProcessComplexEvents.Where(x => x.ProcessId == processId).ToList();

            var lst = processSteps.Select(
                p => new CreateProcessActor($"{p.Name.GetSafeActorName()}:{p.Id}",
                    cpLst,
                    new StateCommandInfo(p.Id, RevolutionData.Context.Actor.Commands.CreateActor),
                    new SystemProcess(
                        new RevolutionEntities.Process.Process(p.Id, p.ParentProcessId, p.Name, p.Description,
                            p.Symbol, user),
                        Source.MachineInfo), Source));
            foreach (var p in lst)
            {
                PublishActor(p);
            }
        }

        private void PublishActor(CreateProcessActor inMsg)
        {
            try
            {
               // var actorName = "ProcessActor-" + inMsg.Process.Name.GetSafeActorName();


                var child = ctx.Child(inMsg.ActorName);
                if (Equals(child, ActorRefs.Nobody))
                {
                     Task.Run(() => { ctx.ActorOf(Props.Create<ProcessActor>(inMsg), inMsg.ActorName); })
                    .ConfigureAwait(false);
                }


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
                        new StateEventInfo(inMsg.Process.Id, RevolutionData.Context.Process.Events.Error)),
                    Source);
            }
        }

        public List<IComplexEventAction> ProcessComplexEvents { get; }
    }


}