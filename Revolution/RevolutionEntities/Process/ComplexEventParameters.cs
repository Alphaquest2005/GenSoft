using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using SystemInterfaces;
using Actor.Interfaces;

namespace RevolutionEntities.Process
{
    public class ComplexEventParameters : IComplexEventParameters
    {
        public ComplexEventParameters() { }
        public ComplexEventParameters(IComplexEventService actor,  ImmutableDictionary<string, dynamic> messages)
        {
            Actor = actor;
            Messages = messages;
           
        }

        public IComplexEventService Actor { get; set; }
        public ImmutableDictionary<string, dynamic> Messages { get; set; }
       //public Action<IComplexEventParameters> Action { get;  }
    }
}