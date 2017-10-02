using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Common;
using Common.DataEntites;
using CommonMessages;
using EFRepository;
using EventAggregator;
using EventMessages;
using EventMessages.Commands;
using EventMessages.Events;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionData;
using RevolutionEntities.Process;
using StartUp.Messages;
using ViewModel.Interfaces;
using Process = RevolutionEntities.Process.Process;


namespace DataServices.Actors
{


    public class ServiceManager : ReceiveActor, IServiceManager
    {

        public ISystemSource Source { get; private set; }

        
        public static List<ISystemProcessInfo> ProcessInfos { get; set; }

       
       
        public ServiceManager(bool autoRun, List<IMachineInfo> machineInfos, List<ISystemProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewModelInfos)
        {
            try
            {

               ProcessInfos = processInfos;
                var ctx = Context;
                var machineInfo =
                    machineInfos.FirstOrDefault(
                        x => x.MachineName == Environment.MachineName && x.Processors == Environment.ProcessorCount);
                if (machineInfo == null) return;
                var processInfo = processInfos.FirstOrDefault(x => x.ParentProcessId == 0);
                if (processInfo == null) return;

                var systemProcess =
                    new SystemProcess(new RevolutionEntities.Process.Process(processInfo, new Agent("System")),
                        machineInfo);
                Source = new Source(Guid.NewGuid(), "ServiceManager", new SourceType(typeof(IServiceManager)),
                    systemProcess, machineInfo);
                var systemStartedMsg =
                    new SystemStarted(
                        new StateEventInfo(systemProcess.Id, RevolutionData.Context.Process.Events.ProcessStarted),
                        systemProcess, Source);

               
                EventMessageBus.Current.GetEvent<IServiceStarted<IEntityDataServiceManager>>(Source)
                    .Where(x => x.Process.Id == 1) //only start up process
                    .Subscribe(x =>
                    {
                        //start the data supervisor
                        EventMessageBus.Current.Publish(
                            new LoadEntitySet(new DynamicEntityType("Test Entity","Test Entities",new List<IEntityKeyValuePair>(),new Dictionary<string, List<dynamic>>(), new ObservableDictionary<string, List<dynamic>>(),new ObservableDictionary<string, string>()), 
                                new StateCommandInfo(systemProcess.Id,
                                    RevolutionData.Context.Entity.Commands.LoadEntitySetWithChanges), systemProcess,
                                Source), Source);

                        EventMessageBus.Current.Publish(
                            new ServiceStarted<IServiceManager>(this,
                                new StateEventInfo(systemProcess.Id,
                                    RevolutionData.Context.Actor.Events.ActorStarted),
                                systemProcess, Source), Source);

                        Task.Run(() => ctx.ActorOf(Props.Create<DomainProcessSupervisor>(autoRun, systemProcess),"DomainProcessSupervisor")).ConfigureAwait(false);

                    });

                EventMessageBus.Current.GetEvent<IServiceStarted<IProcessService>>(Source)
                    .Where(x => x.Process.Id == 1) //only start up process
                    .Subscribe(x =>
                    {
                        Task.Run(() => ctx.ActorOf(Props.Create<EntityDataServiceManager>(systemProcess),
                            "EntityDataServiceManager")).ConfigureAwait(false);
                    });

                EventMessageBus.Current
                    .GetEvent<IServiceStarted<IViewModelSupervisor>>(Source)
                    .Where(z => z.Process.Id == 1).Subscribe(z =>
                    {
                        Task.Run(() => ctx.ActorOf(
                            Props.Create<SystemProcessSupervisor>(autoRun, systemStartedMsg, processInfos,
                                complexEventActions), "ProcessSupervisor")).ConfigureAwait(false);

        

                    });

                Task.Run(() => ctx.ActorOf(
                    Props.Create<ViewModelSupervisor>(viewModelInfos, systemProcess, systemStartedMsg),
                    "ViewModelSupervisor")).ConfigureAwait(false);








            }
            catch (Exception ex)
            {
                Debugger.Break();
                EventMessageBus.Current.Publish(new ProcessEventFailure(
                        failedEventType: typeof(ServiceStarted<IServiceManager>),
                        failedEventMessage: null,
                        expectedEventType: typeof(ServiceStarted<IServiceManager>),
                        exception: ex,
                        source: Source,
                        processInfo: new StateEventInfo(1, RevolutionData.Context.Process.Events.Error)),
                    Source);

            }
        }

      

        public string UserId => this.Source.SourceName;
    }


}
