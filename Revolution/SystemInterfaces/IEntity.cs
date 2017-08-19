using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net.Mime;
using JB.Collections.Reactive;

namespace SystemInterfaces
{

    public interface IDynamicEntity:IEntity
    {
        string EntityType { get; }
        ObservableList<IEntityKeyValuePair> PropertyList { get; }
    }

    public interface IEntityKeyValuePair: INotifyPropertyChanged
    {
        string Key { get; }
        dynamic Value { get; }
    }

    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}