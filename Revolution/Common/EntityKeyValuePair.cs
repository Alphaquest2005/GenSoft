using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SystemInterfaces;
using SystemInterfaces.Annotations;

namespace Common
{

    public class EntityKeyValuePair : IEntityKeyValuePair
    {
        public EntityKeyValuePair(string key, dynamic value, ViewAttributeDisplayProperties displayProperties, bool isEntityId = false, bool isEntityName = false ) 
        {
            Value = value;
            DisplayProperties = displayProperties;
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

       // public ViewAttributeDisplayProperties GridVisiblity => this.DisplayProperties; //new ViewAttributeDisplayProperties(new AttributeDisplayProperties(new Dictionary<string, string>() { { "Visibility", "real shit" } }, null, null),null ); 

        public ViewAttributeDisplayProperties DisplayProperties { get; }

        IViewAttributeDisplayProperties IEntityKeyValuePair.DisplayProperties => DisplayProperties;

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(new EntityKeyValuePair(this.Key, this.Value, this.DisplayProperties), new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AttributeDisplayProperties : IAttributeDisplayProperties
    {
        public AttributeDisplayProperties(Dictionary<string, string> gridProperties, Dictionary<string, string> labelProperties, Dictionary<string, string> valueProperties)
        {
            GridProperties = gridProperties;
            LabelProperties = labelProperties;
            ValueProperties = valueProperties;
        }

        public Dictionary<string, string> GridProperties { get; }
        public Dictionary<string, string> LabelProperties { get; }
        public Dictionary<string, string> ValueProperties { get; }
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