using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SystemInterfaces;
using Actor.Interfaces;
using Common;
using EventAggregator;
using EventMessages.Events;
using Process.WorkFlow;
using RevolutionEntities.Process;
using ViewModel.Interfaces;


namespace DataServices.Actors
{


    public class ServiceManager : IServiceManager
    {

        public ISystemSource Source { get; private set; }

        
        
       
       
        public ServiceManager(bool autoRun, List<IMachineInfo> machineInfos,  List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewModelInfos)
        {
            try
            {

                
                

                if (machineInfos.FirstOrDefault(x => x.MachineName == Processes.ThisMachineInfo.MachineName) == null)
                {
                    Application.Current.Shutdown();
                }
                
                Source = new Source(Guid.NewGuid(), "ServiceManager", new SourceType(typeof(IServiceManager)),
                    Processes.IntialSystemProcess, Processes.ThisMachineInfo);
                var systemStartedMsg =
                    new SystemStarted(
                        new StateEventInfo(Processes.IntialSystemProcess, RevolutionData.Context.Process.Events.ProcessStarted),
                        Processes.IntialSystemProcess, Source);

                Task.Run(() => new EntityDataServiceManager(Processes.IntialSystemProcess)).ConfigureAwait(false);

                Task.Run(() => new DomainProcessSupervisor(autoRun, Processes.IntialSystemProcess)).ConfigureAwait(false);

                Task.Run(() => new SystemProcessSupervisor(systemStartedMsg, complexEventActions)).ConfigureAwait(false);

                Task.Run(() => new ViewModelSupervisor(viewModelInfos, Processes.IntialSystemProcess)).ConfigureAwait(false);


                EventMessageBus.Current.Publish(
                    new ServiceStarted<IServiceManager>(this,
                        new StateEventInfo(Processes.IntialSystemProcess,
                            RevolutionData.Context.Actor.Events.ActorStarted),
                        Processes.IntialSystemProcess, Source), Source);





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
                        processInfo: new StateEventInfo(Processes.IntialSystemProcess, RevolutionData.Context.Process.Events.Error)),
                    Source);

            }
        }

      

        public string UserId => this.Source.SourceName;
    }


}
