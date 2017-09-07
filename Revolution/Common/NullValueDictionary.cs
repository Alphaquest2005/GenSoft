using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;

namespace Common
{

    public class NullValueDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INullValueDictionary<TKey, TValue>
        where TValue : class
    {
        public NullValueDictionary(Dictionary<TKey,TValue> originalDictionary)
        {

            foreach (var itm in originalDictionary)
            {
                this.Add(itm.Key, itm.Value);
            }

        }
        TValue INullValueDictionary<TKey, TValue>.this[TKey key] => Keys.Contains(key)?this[key]:null;
    }
}
