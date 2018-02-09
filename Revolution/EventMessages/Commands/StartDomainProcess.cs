using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using CommonMessages;
using ViewModel.Interfaces;
using IComplexEventAction = Actor.Interfaces.IComplexEventAction;
using IStateCommandInfo = SystemInterfaces.IStateCommandInfo;
using ISystemProcess = SystemInterfaces.ISystemProcess;

namespace EventMessages.Commands
{
    [Export(typeof(ILoadProcessComplexEvents))]

    public class LoadProcessComplexEvents : ProcessSystemMessage, ILoadProcessComplexEvents
    {
        public LoadProcessComplexEvents(){}
       

        public LoadProcessComplexEvents(List<IComplexEventAction> complexEvents, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("LoadDomainProcessComplexEvents", new Dictionary<string, object>() { { "ComplexEvents", complexEvents }}), processInfo, process, source)
        {
            ComplexEvents = complexEvents;
        }

        
        public List<IComplexEventAction> ComplexEvents { get; }
        
    }

    [Export(typeof(ILoadDomainProcessViewModels))]

    public class LoadDomainProcessViewModels : ProcessSystemMessage, ILoadDomainProcessViewModels
    {
        public LoadDomainProcessViewModels(){}


        public LoadDomainProcessViewModels(List<IViewModelInfo> viewModelInfos, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("LoadDomainProcessViewModels", new Dictionary<string, object>() { { "ViewModelInfos", viewModelInfos } }), processInfo, process, source)
        {
            ViewModelInfos = viewModelInfos;
        }
        public List<IViewModelInfo> ViewModelInfos { get; }
    }

    [Export(typeof(ILoadDataService))]

    public class LoadDataService : ProcessSystemMessage, ILoadDataService
    {
        public LoadDataService(){}

        public LoadDataService(string entityTypeName, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            : base(new DynamicObject("LoadDataService", new Dictionary<string, object>() { { "EntityTypeName", entityTypeName } }), processInfo, process, source)
        {
            EntityTypeName = entityTypeName;
        }
        public string EntityTypeName { get; }
    }
}