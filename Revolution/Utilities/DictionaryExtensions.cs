using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Utilities
{
    public static class DictionaryExtensions
    {

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,TKey key,TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue: class
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : null;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,TKey key,Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value)
                ? value
                : defaultValueProvider();
        }

            // Either Add or overwrite
            public static void AddOrUpdate<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
            {
                //if (dictionary == null) dictionary = new ConcurrentDictionary<K, V>();
                dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
            }

        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            //if (dictionary == null) dictionary = new ConcurrentDictionary<K, V>();
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
            
        }

    }
}
