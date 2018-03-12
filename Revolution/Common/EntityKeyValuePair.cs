using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Windows;
using SystemInterfaces;

namespace Common
{

    public class EntityKeyValuePair : IEntityKeyValuePair
    {
        public EntityKeyValuePair(string key, dynamic value, ViewAttributeDisplayProperties displayProperties, bool isComputed, bool isEntityId = false, bool isEntityName = false ) 
        {
            Contract.Requires(key != null);
            Value = value;
            DisplayProperties = displayProperties;
            IsComputed = isComputed;
            Key = key;
            if (isEntityId) IsEntityId = true;
            if (isEntityName) IsEntityId = true;
        }

        public string Key { get; set; }


        public bool IsEntityId { get; set; }
        public bool IsEntityName { get; set; }

        private dynamic _value;

        public dynamic Value
        {
            get => _value;
            set
            {
                _value = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.OnPropertyChanged();
                });
            }
        }

        public ViewAttributeDisplayProperties DisplayProperties { get; }
        public bool IsComputed { get; }
        
        IViewAttributeDisplayProperties IEntityKeyValuePair.DisplayProperties => DisplayProperties;

        
        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
           
            PropertyChanged?.Invoke(new EntityKeyValuePair(this.Key??"UnknownKey", this.Value ?? "Unknown Value", DisplayProperties, this.IsComputed), new PropertyChangedEventArgs(propertyName));
        }
    }


    public class AttributeDisplayProperties : IAttributeDisplayProperties
    {
        public AttributeDisplayProperties(Dictionary<string, Dictionary<string, string>> properties)
        {
            Properties = properties;
        }

        public Dictionary<string, Dictionary<string, string>> Properties { get; }

    }

    public class ViewAttributeDisplayProperties : IViewAttributeDisplayProperties
    {
        public ViewAttributeDisplayProperties(AttributeDisplayProperties readProperties, AttributeDisplayProperties writeProperties)
        {
            ReadProperties = readProperties;
            WriteProperties = writeProperties;
        }

        public AttributeDisplayProperties ReadProperties { get; }
        public AttributeDisplayProperties WriteProperties { get; }

        IAttributeDisplayProperties IViewAttributeDisplayProperties.ReadProperties => ReadProperties;

        IAttributeDisplayProperties IViewAttributeDisplayProperties.WriteProperties => WriteProperties;
    }
}