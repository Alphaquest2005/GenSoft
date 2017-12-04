using System.Collections.Immutable;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    //public interface IDynamicComplexEventParameters
    //{
    //    IComplexEventService Actor { get; }
    //    ImmutableDictionary<string, dynamic> Messages { get; }
    //}

    public interface IDynamicComplexEventParameters//:IDynamicComplexEventParameters
    {
        IComplexEventService Actor { get; }
        ImmutableDictionary<string, IDynamicObject> Messages { get; }
    }
}