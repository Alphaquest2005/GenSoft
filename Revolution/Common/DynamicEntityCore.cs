using System.Diagnostics.Contracts;
using SystemInterfaces;
using Common.Dynamic;

namespace Common.DataEntites
{
    public class DynamicEntityCore : Expando, IDynamicEntityCore
    {
        public DynamicEntityCore(IDynamicEntityType entityType, int id)
        {
            Contract.Requires(entityType != null, "EntityType is Null");
            EntityType = entityType;
            Id = id;

        }
        public int Id { get; }
       
        public virtual RowState RowState { get; set; } = RowState.Loaded;
        public IDynamicEntityType EntityType { get; }

       //private readonly Guid _entityGuid = Guid.NewGuid();

        public override bool Equals(object obj)
        {

            var other = obj as DynamicEntityCore;
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
           // if (Id == 0 || other.Id == 0) return false;
            return Id == other.Id;
        }

        public static bool operator ==(DynamicEntityCore a, DynamicEntityCore b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(DynamicEntityCore a, DynamicEntityCore b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once cuz of nhibernate
            // return (_entityGuid.ToString()).GetHashCode();
            return (Id.ToString()).GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DynamicEntityCore;
            if (ReferenceEquals(other, null)) return -1;
            return Id.CompareTo(other.Id);
        }
    }
}