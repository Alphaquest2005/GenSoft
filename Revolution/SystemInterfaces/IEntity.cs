using System.Collections.Generic;
using System.ComponentModel.Composition;
using JB.Collections.Reactive;

namespace SystemInterfaces
{

    public interface IDynamicEntity:IEntity
    {
        string EntityType { get; }
        List<KeyValuePair<string, object>> PropertyList { get; }
    }
    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}