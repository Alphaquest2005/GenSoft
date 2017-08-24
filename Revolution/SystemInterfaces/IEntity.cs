using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net.Mime;
using JB.Collections.Reactive;

namespace SystemInterfaces
{

    public interface IDynamicEntity:IEntity
    {
        IDynamicEntityType EntityType { get; }
        ObservableList<IEntityKeyValuePair> PropertyList { get; }
    }


    public interface IEntityKeyValuePair: INotifyPropertyChanged
    {
        string Key { get; }
        bool IsEntityId { get; }
        bool IsEntityName { get; }
       dynamic Value { get; }
    }

    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}