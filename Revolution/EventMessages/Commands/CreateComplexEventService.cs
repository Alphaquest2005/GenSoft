using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Actor.Interfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{

    [Export(typeof(ICreateComplexEventService))]
    public class CreateComplexEventService:ProcessSystemMessage, ICreateComplexEventService
    {
        public CreateComplexEventService() { }
        public CreateComplexEventService(IComplexEventService complexEventService, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("CreateComplexEventService", new Dictionary<string, object>() { { "ComplexEventService", complexEventService } }),processInfo,process, source)
        {
            ComplexEventService = complexEventService;
        }

        public IComplexEventService ComplexEventService { get;  }
    }
}
