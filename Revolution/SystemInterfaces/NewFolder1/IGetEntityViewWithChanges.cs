using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SystemInterfaces
{
    
    public interface IGetEntityViewWithChanges<out TEntityView> : IMessage, IEntityViewRequest<TEntityView> where TEntityView : IEntityView
    {
        Dictionary<string, object> Changes { get; }
        
    }


    public interface ILoadPulledEntityViewSetWithChanges<out TMatchType> : IMessage, IEntityViewRequest where TMatchType : IMatchType
    {
        Dictionary<string, dynamic> Changes { get; }
        string EntityName { get; }
        Type ViewType { get; }

    }
}