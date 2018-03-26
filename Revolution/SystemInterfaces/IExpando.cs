using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SystemInterfaces
{
    public interface IExpando
    {
        /// <summary>
        /// Instance of object passed in
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// Return both instance and dynamic names.
        /// 
        /// Important to return both so JSON serialization with 
        /// Json.NET works.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetDynamicMemberNames();

        /// <summary>
        /// Try to retrieve a member by name first from instance properties
        /// followed by the collection entries.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryGetMember(GetMemberBinder binder, out dynamic result);

        /// <summary>
        /// Property setter implementation tries to retrieve value from instance 
        /// first then into this object
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TrySetMember(SetMemberBinder binder, dynamic value);

        /// <summary>
        /// Dynamic invocation method. Currently allows only for Reflection based
        /// operation (no ability to add methods dynamically).
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryInvokeMember(InvokeMemberBinder binder, dynamic[] args, out dynamic result);

        /// <summary>
        /// Convenience method that provides a string Indexer 
        /// to the Properties collection AND the strongly typed
        /// properties of the object by name.
        /// 
        /// // dynamic
        /// exp["Address"] = "112 nowhere lane"; 
        /// // strong
        /// var name = exp["StronglyTypedProperty"] as string; 
        /// </summary>
        /// <remarks>
        /// The getter checks the Properties dictionary first
        /// then looks in PropertyInfo for properties.
        /// The setter checks the instance properties before
        /// checking the Properties dictionary.
        /// </remarks>
        /// <param name="key"></param>
        /// 
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Returns and the properties of 
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, object>> GetProperties(bool includeInstanceProperties = false);

        /// <summary>
        /// Checks whether a property exists in the Property collection
        /// or as a property on the instance
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(KeyValuePair<string, dynamic> item, bool includeInstanceProperties = false);

        event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null);
        bool TryDeleteMember(DeleteMemberBinder binder);
        bool TryConvert(ConvertBinder binder, out dynamic result);
        bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out dynamic result);
        bool TryInvoke(InvokeBinder binder, object[] args, out dynamic result);
        bool TryBinaryOperation(BinaryOperationBinder binder, dynamic arg, out dynamic result);
        bool TryUnaryOperation(UnaryOperationBinder binder, out dynamic result);
        bool TryGetIndex(GetIndexBinder binder, dynamic[] indexes, out dynamic result);
        bool TrySetIndex(SetIndexBinder binder, dynamic[] indexes, dynamic value);
        bool TryDeleteIndex(DeleteIndexBinder binder, dynamic[] indexes);
        DynamicMetaObject GetMetaObject(Expression parameter);

        Dictionary<string, IDynamicValue> Properties { get; }
    }
}
