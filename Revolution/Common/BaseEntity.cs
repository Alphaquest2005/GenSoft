using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using SystemInterfaces;
using Common.Dynamic;
using JB.Collections.Reactive;

namespace Common.DataEntites
{

    public static class DynamicEntityTypeExtenstions
    {
        public static IDynamicEntity DefaultEntity(this IDynamicEntityType dt)
        {
             return new DynamicEntity(dt, 0, dt.Properties.ToDictionary(x => x.Key, x => x.Value)); 
        }
    }
    public class DynamicEntityType : IDynamicEntityType
    {
        public static Dictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new Dictionary<string, IDynamicEntityType>();

        
        public DynamicEntityType(string name, List<IEntityKeyValuePair> properties)
        {
            Name = name;
            Properties = properties;
        }

        

        public string Name { get; }
        public List<IEntityKeyValuePair> Properties { get; }
    }
    public class DynamicEntity:Expando, IDynamicEntity
    {

        
        
        public DynamicEntity(IDynamicEntityType entityType, int id, Dictionary<string,object> values)
        {
            Contract.Requires(entityType != null, "EntityType is Null");
            EntityType = entityType;
            Id = id;
            
            foreach (var itm in entityType.Properties)
            {
                Properties.Add(itm.Key, values[itm.Key]);
            }

            
        }
        public int Id { get;  }
        public DateTime EntryDateTime { get; private set; } = DateTime.Now;

        public virtual IDynamicEntityType EntityType { get; }
        public ObservableList<IEntityKeyValuePair> PropertyList => new ObservableList<IEntityKeyValuePair>(this.Properties.Select(x => new EntityKeyValuePair(x.Key, x.Value) as IEntityKeyValuePair).ToList());


        public virtual RowState RowState { get; set; } = RowState.Loaded ;

        private string _entityName;
        public dynamic EntityName
        {
            get
            {
                if(_entityName == null) _entityName = EntityType.Properties.FirstOrDefault(x => x.IsEntityName)?.Key;
                return _entityName == null ? this.Properties["EntityName"] : Properties[_entityName].ToString();
            }
            set
            {
                if (_entityName == null) _entityName = EntityType.Properties.FirstOrDefault(x => x.IsEntityName)?.Key;
                if (_entityName == null) this.Properties["EntityName"] = value;
                else Properties[_entityName] = value;
            }
        }

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
