using System.Collections.Generic;
using System.ComponentModel;
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
        Dictionary<string, object> Properties { get; }
        
    }


    public interface IDynamicObject 
    {
        string Type { get; }
        Dictionary<string, IDynamicValue> Properties { get; }
    }

    public interface IDynamicType
    {
        string Type { get; }
        Dictionary<string, object> Properties { get; }
        int Id { get; }
    }

    public interface IDynamicValue
    {
        string Type { get; }
        object Value { get; }
        TValue GetValue<TValue>();
    }

    public interface IEntityKeyValuePair : INotifyPropertyChanged
    {
        string Key { get; }
        bool IsEntityId { get; }
        bool IsEntityName { get; }
        dynamic Value { get; }
        IViewAttributeDisplayProperties DisplayProperties { get; }
        bool IsComputed { get; }
    }
    public interface IViewAttributeDisplayProperties
    {
        IAttributeDisplayProperties ReadProperties { get; }
        IAttributeDisplayProperties WriteProperties { get; }

    }

    public interface IAttributeDisplayProperties
    {
        Dictionary<string, Dictionary<string, string>> Properties { get; }
    }


    public interface IEntity:IEntityId
    {
        RowState RowState { get; set; }

        
    }
}