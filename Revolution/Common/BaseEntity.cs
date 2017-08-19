using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using SystemInterfaces;
using Common.Dynamic;
using JB.Collections.Reactive;

namespace Common.DataEntites
{
   
    public class DynamicEntity:Expando, IDynamicEntity
    {
        public DynamicEntity(string entityType, int id, List<EntityKeyValuePair> toList = null)
        {
            EntityType = entityType;
            Id = id;
            if (toList == null) return;
            foreach (var itm in toList)
            {
                Properties.Add(itm.Key, itm.Value);
            }
        }
        public int Id { get;  }
        public DateTime EntryDateTime { get; private set; } = DateTime.Now;

        public virtual string EntityType { get; }
        public ObservableList<IEntityKeyValuePair> PropertyList => new ObservableList<IEntityKeyValuePair>(this.Properties.Select(x => new EntityKeyValuePair(x.Key, x.Value) as IEntityKeyValuePair).ToList());


        public virtual RowState RowState { get; set; } = RowState.Loaded ;
        
        private readonly Guid _entityGuid = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            
            var other = obj as DynamicEntity;
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if(GetType() != other.GetType()) return false;
            if (Id == 0 || other.Id == 0) return false;
            return Id == other.Id;
        }

        public static bool operator ==(DynamicEntity a, DynamicEntity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(DynamicEntity a, DynamicEntity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once cuz of nhibernate
            return (_entityGuid.ToString()).GetHashCode();
        }

        
    }

    public abstract class BaseEntity : IEntity
    {
       
        public int Id { get; set; }
        public DateTime EntryDateTime { get; private set; } = DateTime.Now;

        [IgnoreDataMember]
        [NotMapped]
        public virtual RowState RowState { get; set; } = RowState.Loaded;

        private readonly Guid _entityGuid = Guid.NewGuid();

        public override bool Equals(object obj)
        {

            var other = obj as DynamicEntity;
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (Id == 0 || other.Id == 0) return false;
            return Id == other.Id;
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once cuz of nhibernate
            return (_entityGuid.ToString()).GetHashCode();
        }


    }
}
