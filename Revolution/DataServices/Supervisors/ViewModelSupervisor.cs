using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;

using EventAggregator;
using EventMessages.Events;
using RevolutionEntities.Process;
using ViewMessages;
using ViewModel.Interfaces;

namespace DataServices.Actors
{
  
    public class ViewModelSupervisor : BaseSupervisor<ViewModelSupervisor>, IViewModelSupervisor
    {
        
       

        public ViewModelSupervisor(List<IViewModelInfo> processViewModelInfos, ISystemProcess process) : base(process)
        {
           HandleProcessViews(processViewModelInfos);
            EventMessageBus.Current.GetEvent<ILoadDomainProcessViewModels>(new RevolutionEntities.Process.StateCommandInfo(process, RevolutionData.Context.CommandFunctions.UpdateCommandStatus("shit can't think", RevolutionData.Context.Process.Commands.StartProcess)), Source).Subscribe(x => HandleProcessViews(x.ViewModelInfos));
            
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
                    var t = new ViewModelActor(msg, v.Process);
                   // EventMessageBus.Current.Publish(msg, Source);
                });
        }


    }

}