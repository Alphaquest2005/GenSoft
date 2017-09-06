using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SystemInterfaces;
using GenSoft.Interfaces;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
        public static Dictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new Dictionary<string, IDynamicEntityType>();

        public static Dictionary<IEntityTypeAttributes, Expression<Func<IDynamicEntity, dynamic>>> CalulatedProperties { get; } 
            = new Dictionary<IEntityTypeAttributes, Expression<Func<IDynamicEntity, dynamic>>>();

        public static Dictionary<string, dynamic> Functions { get; } = new Dictionary<string, dynamic>();

        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, bool isList, bool isParentEntity)
        {
            Name = name;
            Properties = properties;
            IsList = isList;
            IsParentEntity = isParentEntity;
            EntitySetName = entitySetName;
        }

        

        public string Name { get; }
        public bool IsList { get; }
        public string EntitySetName { get; }
        public List<IEntityKeyValuePair> Properties { get; }
        public bool IsParentEntity { get; }
    }

}