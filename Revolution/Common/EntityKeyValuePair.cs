using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SystemInterfaces;
using SystemInterfaces.Annotations;

namespace Common
{
    public class EntityKeyValuePair : IEntityKeyValuePair
    {
        public EntityKeyValuePair(string key, dynamic value)
        {
            Value = value;
            Key = key;
        }

        private string _key;

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged();
            }
        }

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


        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
           PropertyChanged?.Invoke(new EntityKeyValuePair(this.Key, this.Value), new PropertyChangedEventArgs(propertyName));
        }
    }
}