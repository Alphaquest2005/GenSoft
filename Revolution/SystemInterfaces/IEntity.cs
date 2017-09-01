using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net.Mime;
using JB.Collections.Reactive;

namespace SystemInterfaces
{
    public interface IDynamicEntityCore : IEntity
    {
        IDynamicEntityType EntityType { get; }
        
    }


    public interface IDynamicEntity: IDynamicEntityCore
    {
        ObservableList<IEntityKeyValuePair> PropertyList { get; }
    }


    public interface IEntityKeyValuePair : INotifyPropertyChanged
    {
        string Key { get; }
        bool IsEntityId { get; }
        bool IsEntityName { get; }
        dynamic Value { get; }
        IViewAttributeDisplayProperties DisplayProperties { get; }

    }

    public interface IViewAttributeDisplayProperties
    {
        IAttributeDisplayProperties ReadProperties { get; }
        IAttributeDisplayProperties WriteProperties { get; }

    }

    public interface IAttributeDisplayProperties
    {
        Dictionary<string, string> GridProperties { get; }
        Dictionary<string, string> LabelProperties { get; }
        Dictionary<string, string> ValueProperties { get; }
    }

    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}