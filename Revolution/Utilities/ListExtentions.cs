using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;

namespace Utilities
{
    public static class ListExtentions
    {
        public static void AddOrUpdate(this IList<IDynamicEntity> source, IDynamicEntity item)
        {
            var existingEntity = source.FirstOrDefault(x => x.Id == item.Id && x.EntityType == item.EntityType);
            if (existingEntity == null)
            {
                source.Add(item);
            }
            else
            {
                var idx = source.IndexOf(existingEntity);
                source.Remove(existingEntity);
                source.Insert(idx, item);
            }
        }

        
    }

    public class InteliList<T> : List<T>, IIntelliList<T>
    {
        public T SelectedItem { get; set; }
    }
}
