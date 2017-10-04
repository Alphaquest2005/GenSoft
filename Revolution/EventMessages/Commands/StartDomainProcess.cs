using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Actor.Interfaces;
using CommonMessages;
using GenSoft.Interfaces;
using ViewModel.Interfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace EventMessages.Commands
{
    [Export(typeof(ILoadDomainProcess))]

    public class LoadDomainProcess : ProcessSystemMessage, ILoadDomainProcess
    {
        public LoadDomainProcess(IDomainProcess domainProcess, List<IComplexEventAction> complexEvents, List<IViewModelInfo> viewModelInfos)
        {
            DomainProcess = domainProcess;
            ComplexEvents = complexEvents;
            ViewModelInfos = viewModelInfos;
        }
       

        public LoadDomainProcess(IDomainProcess domainProcess, List<IComplexEventAction> complexEvents, List<IViewModelInfo> viewModelInfos, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            DomainProcess = domainProcess;
            ComplexEvents = complexEvents;
            ViewModelInfos = viewModelInfos;
        }

        public IDomainProcess DomainProcess { get; }
        public List<IComplexEventAction> ComplexEvents { get; }
        public List<IViewModelInfo> ViewModelInfos { get; }
    }
}