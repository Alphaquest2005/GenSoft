using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using JB.Collections.Reactive;

namespace Common.DataEntites
{
    public class DynamicEntity:DynamicEntityCore, IDynamicEntity
    {
        public DynamicEntity(IDynamicEntityType entityType, int id, Dictionary<string,object> values) : base(entityType,id)
        {
            
            foreach (var itm in entityType.Properties.Where(x =>x.Key != nameof(IDynamicEntity.Id)))
            {
                Properties.Add(itm.Key, values.ContainsKey(itm.Key)? values[itm.Key]: null);
            }

            
        }
        
        public ObservableList<IEntityKeyValuePair> PropertyList => new ObservableList<IEntityKeyValuePair>(this.Properties.Where(x => x.Key != nameof(IDynamicEntity.Id)).Select(x => new EntityKeyValuePair(x.Key, x.Value) as IEntityKeyValuePair).ToList());

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


        //public override bool Equals(object obj)
        //{
        //    var other = obj as DynamicEntity;
        //    return Equals(other);
        //}

        //protected bool Equals(DynamicEntity other)
        //{
        //    return base.Equals(other) ;//&& PropertyList.SequenceEqual(other.PropertyList);
        //}

        //public override int GetHashCode()
        //{
        //    try
        //    {
        //        return (int) (base.GetHashCode() ^ (PropertyList.Any() ? PropertyList.GetHashCode() : 0));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }


        //}

        //public static bool operator ==(DynamicEntity a, DynamicEntity b)
        //{
        //    if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
        //    if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        //    return a.Equals(b);
        //}

        //public static bool operator !=(DynamicEntity a, DynamicEntity b)
        //{
        //    return !(a == b);
        //}
    }
}