using System;
using SystemInterfaces;
using Utilities;

namespace Common.DataEntites
{
    public class DynamicValue : IDynamicValue
    {
        public DynamicValue(Type type, object value)
        {
            Type = type?.FullName;
            Value = value;
        }

        public string Type { get; }
        public object Value { get; }
        public TValue GetValue<TValue>()
        {
            return (TValue) Convert.ChangeType(Value, TypeNameExtensions.GetTypeByName(Type)[0]);
        }
    }
}