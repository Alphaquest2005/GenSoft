using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Events
{


    [Export(typeof(IServiceStarted<>))]
    public class ServiceStarted<TService> : ProcessSystemMessage, IServiceStarted<TService>
    {
        public ServiceStarted() { }
        public ServiceStarted(TService service, IStateEventInfo processInfo, ISystemProcess process, ISystemSource source) 
            : base(new DynamicObject("ServiceStarted", new Dictionary<string, object>() { { "Service", service }}), processInfo, process, source)
        {
            Service = service;
        }

        public TService Service { get; }
    }
}
