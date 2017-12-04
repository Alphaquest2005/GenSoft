using System.Collections.Immutable;
using SystemInterfaces;
using Actor.Interfaces;

namespace RevolutionEntities.Process
{

    public class DynamicComplexEventParameters : IDynamicComplexEventParameters
    {
        public DynamicComplexEventParameters() { }
        public DynamicComplexEventParameters(IComplexEventService actor, ImmutableDictionary<string, IDynamicObject> messages)
        {
            Actor = actor;
            Messages = messages;

        }

        public IComplexEventService Actor { get; set; }
        public ImmutableDictionary<string, IDynamicObject> Messages { get; set; }
        //public Action<IDynamicComplexEventParameters> Action { get;  }
    }

}