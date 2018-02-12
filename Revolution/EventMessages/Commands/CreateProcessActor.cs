using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using CommonMessages;
using Actor.Interfaces;
using Common.DataEntites;

namespace EventMessages.Commands
{

    [Export(typeof(ICreateProcessActor))]
    public class CreateProcessActor:ProcessSystemMessage, ICreateProcessActor
    {
        public CreateProcessActor(){}

        public CreateProcessActor(string actorName, List<IComplexEventAction> complexEvents, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source)
            :base(new DynamicObject("CreateProcessActor", new Dictionary<string, object>() { { "ActorName", actorName }, { "ComplexEvents", complexEvents } }), processInfo,process, source)
        {
            ComplexEvents = complexEvents;
            ActorId = actorName;
        }

        public string ActorId { get; }
        public IReadOnlyList<IComplexEventAction> ComplexEvents { get; }
    }
}
