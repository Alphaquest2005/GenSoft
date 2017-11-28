using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using SystemInterfaces;
using DynamicExpresso;
using GenSoft.Interfaces;
using JB.Collections.Reactive;
using Utilities;

namespace Common.DataEntites
{
    public class DynamicEntityType : IDynamicEntityType
    {
        public static ConcurrentDictionary<string,IDynamicEntityType> DynamicEntityTypes { get; } = new ConcurrentDictionary<string, IDynamicEntityType>();

        
        

        public DynamicEntityType(string name, string entitySetName, List<IEntityKeyValuePair> properties, Dictionary<string, List<dynamic>> calculatedProperties, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties, ObservableDictionary<string, string> propertyParentEntityType)
        {
            Name = name;
            Properties = properties;
            PropertyParentEntityType = propertyParentEntityType;
            CachedProperties = cachedProperties;
            CalculatedProperties = calculatedProperties;
            EntitySetName = entitySetName;
        }

        

        public string Name { get; }
        public string EntitySetName { get; }
        public List<IEntityKeyValuePair> Properties { get; }
        public Dictionary<string, List<dynamic>> CalculatedProperties { get; }
        public ObservableDictionary<string, Dictionary<int, dynamic>> CachedProperties { get; }
        public ObservableDictionary<string, string> PropertyParentEntityType { get; }
        public IIntelliList<IDynamicEntityType> ChildEntities { get; } = new InteliList<IDynamicEntityType>();
        public IIntelliList<IDynamicEntityType> ParentEntities { get; } = new InteliList<IDynamicEntityType>();
    }

}