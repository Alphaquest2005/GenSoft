﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Routing;
using CommonMessages;
using EventAggregator;
using EventMessages.Events;
using RevolutionData;
using RevolutionEntities.Process;
using StartUp.Messages;
using ViewMessages;
using ViewModel.Interfaces;

namespace DataServices.Actors
{
  
    public class ViewModelSupervisor : BaseSupervisor<ViewModelSupervisor>, IViewModelSupervisor
    {

        
        private IUntypedActorContext ctx = null;

        public ViewModelSupervisor(List<IViewModelInfo> processViewModelInfos, ISystemProcess process) : base(process)
        {
            ProcessViewModelInfos = processViewModelInfos;
            ctx = Context;
            Task.Run(() =>
            {
                ctx.ActorOf(
                    Props.Create<ViewModelActor>(process)
                        .WithRouter(new RoundRobinPool(1,
                            new DefaultResizer(1, Environment.ProcessorCount, 1, .2, .3, .1, Environment.ProcessorCount))),"ViewModelActorEntityActor");
            });
            

            EventMessageBus.Current.GetEvent<ISystemProcessStarted>(Source).Subscribe(x => HandleProcessViews(x));
            EventMessageBus.Current.GetEvent<ILoadDomainProcess>(Source).Subscribe(x => HandleDomainViews(x));


            EventMessageBus.Current.GetEvent<IServiceStarted<IViewModelService>>(Source).Subscribe(x =>
            {
                EventMessageBus.Current.Publish(new ServiceStarted<IViewModelSupervisor>(this,new StateEventInfo(process.Id, RevolutionData.Context.Actor.Events.ActorStarted), process, Source), Source);
            });

            
        }

        private void HandleDomainViews(ILoadDomainProcess loadDomainProcess)
        {
            try
            {
                foreach (var itm in ProcessViewModelInfos.Where(x => x.ProcessId == loadDomainProcess.Process.Id).ToList())
                {
                    ProcessViewModelInfos.Remove(itm);
                }
                
                ProcessViewModelInfos.AddRange(loadDomainProcess.ViewModelInfos);

            }
            catch (Exception ex)
            {
                //todo: need  to fix this
                PublishProcesError(loadDomainProcess, ex, loadDomainProcess.GetType());
            }
        }


        private void HandleProcessViews(ISystemProcessStarted pe)
        {
            try
            {
                //Parallel.ForEach(ProcessViewModelInfos.Where(x => x.ProcessId == pe.Process.Id),new ParallelOptions() {MaxDegreeOfParallelism = Environment.ProcessorCount},
                //    (v) =>
                foreach (var v in ProcessViewModelInfos.Where(x => x.ProcessId == pe.Process.Id))
               
                {
                    var msg = new LoadViewModel(v,
                        new StateCommandInfo(pe.Process.Id, RevolutionData.Context.ViewModel.Commands.LoadViewModel),
                        pe.Process, Source);

                    EventMessageBus.Current.Publish(msg, Source);
                }

                //});
            }
            catch (Exception ex)
            {
                //todo: need  to fix this
                PublishProcesError(pe, ex, pe.GetType());
            }

        }

        public List<IViewModelInfo> ProcessViewModelInfos { get;  }
    }

}