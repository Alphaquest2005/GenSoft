using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IComplexEventParameters
    {
        IComplexEventService Actor { get; }
        ImmutableDictionary<string, dynamic> Messages { get; }
    }

    public interface IDynamicComplexEventParameters:IComplexEventParameters
    {
        new ImmutableDictionary<string, IExpando> Messages { get; }
    }
}