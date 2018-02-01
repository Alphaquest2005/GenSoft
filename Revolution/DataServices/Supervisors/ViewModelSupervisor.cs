using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Routing;
using EventAggregator;
using EventMessages.Events;
using RevolutionEntities.Process;
using ViewMessages;
using ViewModel.Interfaces;

namespace DataServices.Actors
{
  
    public class ViewModelSupervisor : BaseSupervisor<ViewModelSupervisor>, IViewModelSupervisor
    {
        private IActorRef _viewActor;
       

        public ViewModelSupervisor(List<IViewModelInfo> processViewModelInfos, ISystemProcess process) : base(process)
        {
            _viewActor = Context.ActorOf(
                    Props.Create<ViewModelActor>(process)
                        .WithRouter(new RoundRobinPool(1,new DefaultResizer(1, Environment.ProcessorCount, 1, .2, .3, .1, Environment.ProcessorCount)))
                        ,"ViewModelActorEntityActor");
          

            HandleProcessViews(processViewModelInfos);
            EventMessageBus.Current.GetEvent<ILoadDomainProcessViewModels>(Source).Subscribe(x => HandleProcessViews(x.ViewModelInfos));
            
            EventMessageBus.Current.Publish(new ServiceStarted<IViewModelSupervisor>(this,new StateEventInfo(process, RevolutionData.Context.Actor.Events.ActorStarted), process, Source), Source);
        }

        private void HandleProcessViews(List<IViewModelInfo> processViewModelInfos)
        {
            Parallel.ForEach(processViewModelInfos,
                new ParallelOptions() {MaxDegreeOfParallelism = Environment.ProcessorCount},
                (v) =>
                {
                    var msg = new LoadViewModel(v,
                        new StateCommandInfo(v.Process, RevolutionData.Context.ViewModel.Commands.LoadViewModel),
                        v.Process, Source);
                    _viewActor.Tell(msg);
                   // EventMessageBus.Current.Publish(msg, Source);
                });
        }


    }

}