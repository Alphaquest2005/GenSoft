using System;
using System.Collections.Generic;

namespace GenSoft.Interface
{
    public class EntityIdComparer : IComparer<IEntityId>, IEqualityComparer<IEntityId>
    {
        public int Compare(IEntityId x, IEntityId y)
        {
            return String.CompareOrdinal(x.Id.ToString(), y.Id.ToString());
        }

        public bool Equals(IEntityId x, IEntityId y)
        {
            return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode(IEntityId obj)
        {
            return obj.GetHashCode();
        }

    }
}