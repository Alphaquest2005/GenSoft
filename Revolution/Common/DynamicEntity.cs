using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using SystemInterfaces;
using Common.Dynamic;
using JB.Collections.Reactive;



namespace Common.DataEntites
{
    public class DynamicEntity:DynamicEntityCore, IDynamicEntity
    {
        public DynamicEntity(IDynamicEntityType entityType, int id, Dictionary<string,object> values) : base(entityType,id)
        {
            
            foreach (var itm in entityType.Properties)
            {
                Properties.Add(itm.Key, values.ContainsKey(itm.Key)? values[itm.Key]: null);
            }

            SetCalculatedProperties();
        }

        
        public ObservableList<IEntityKeyValuePair> PropertyList => new ObservableList<IEntityKeyValuePair>(EntityType.Properties.Where(x => x.Key != null)
            .Select(x => new EntityKeyValuePair(x.Key,Properties[x.Key],
                        (ViewAttributeDisplayProperties) EntityType.Properties.FirstOrDefault(z => z.Key == x.Key)?.DisplayProperties,x.IsComputed,
                        x.IsEntityId,
                        x.IsEntityName)as IEntityKeyValuePair).ToList());

        public new Dictionary<string, object> Properties => base.Properties;

        private string _entityName;
        public string EntityName
        {
            get
            {
                if(_entityName == null) _entityName = EntityType.Properties.FirstOrDefault(x => x.Key == "Name")?.Key ?? EntityType.Properties.FirstOrDefault(x => x.IsEntityName)?.Key ?? EntityType.Properties.FirstOrDefault(x => x.IsEntityId)?.Key;
                return _entityName != null 
                        ?this.Properties[_entityName].ToString()
                        : Properties["Id"].ToString();
            }
            set
            {
                if (_entityName == null) _entityName = EntityType.Properties.FirstOrDefault(x => x.Key == "Name")?.Key ?? EntityType.Properties.FirstOrDefault(x => x.IsEntityName)?.Key;
                if (_entityName != null) Properties[_entityName] = value;
                else this.Properties.Add("Name",value);
            }
        }
        // todo: dual implementation of DynamicEntityType
        public static IDynamicEntity NullEntity => new DynamicEntity(DynamicEntityType.NullEntityType(), 0, new Dictionary<string, object>());


        private void SetCalculatedProperties()
        {
            try
            {
                if (this.Id <= 0) return;
                foreach (var cp in EntityType.CalculatedProperties)
                {
                    dynamic res = this;
                    foreach (var f in cp.Value)
                    {
                        res = f.Invoke(res);
                        if (res == null) break;
                    }
                    Properties[cp.Key] = res;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
        //        return (int)(base.GetHashCode() ^ (PropertyList.Any() ? PropertyList.GetHashCode() : 0));
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

    public class DynamicObject : IDynamicObject
    {
        public DynamicObject(string type, Dictionary<string, object> values)
        {
            Type = type;
            Properties = values.ToDictionary(x => x.Key, z => new DynamicValue(z.Value?.GetType(), z.Value) as IDynamicValue);
        }

        public string Type { get; }
        public Dictionary<string, IDynamicValue> Properties { get; }
    }

    public class DynamicType : Expando,  IDynamicType//, IDynamicMetaObjectProvider
    {
        public DynamicType(string type, int id)
        {
            Type = type;
            Id = id;
            //Properties = values.ToDictionary(x => x.Key, z => z.Value);
        }

        public string Type { get; }

         Dictionary<string, dynamic>  IDynamicType.Properties => base.Properties.ToDictionary(x => x.Key, x => x.Value);
        public int Id { get; }
    }

    public static class IEntityExtensions
    {
        public static IDynamicEntity ToDynamicEntity(this IEntity entity, IDynamicEntityType entityType)
        {
            var props = entity.GetType().GetProperties().Where(x => entityType.Properties.Any(z => z.Key == x.Name))
                .ToList();
            var vals = entityType.Properties
                .Select(x => new {x.Key, Value = props.First(z => z.Name == x.Key).GetValue(entity)})
                .ToDictionary(k => k.Key, v => v.Value);
            var res = new DynamicEntity(entityType, entity.Id, vals);
            return res;
        }
    }
}