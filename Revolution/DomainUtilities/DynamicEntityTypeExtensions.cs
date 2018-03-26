using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
    using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemInterfaces;
using BootStrapper;
using Common;
using Common.DataEntites;
using DynamicExpresso;
using GenSoft.DBContexts;
using GenSoft.Entities;
using JB.Collections.Reactive;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using RevolutionEntities;
using Utilities;
using AddinAction = RevolutionEntities.AddinAction;
using CommandType = System.Data.CommandType;
using Type = System.Type;

namespace DomainUtilities
{
    public static class DynamicEntityTypeExtensions
    {

        public static Dictionary<string, dynamic> Functions { get; } = new Dictionary<string, dynamic>();
        public static Dictionary<string, dynamic> Actions { get; } = new Dictionary<string, dynamic>();

        public static IDynamicEntity DefaultEntity(this IDynamicEntityType dt)
        {
            return new DynamicEntity(dt, 0, dt.Properties.ToDictionary(x => x.Key, x => x.Value));
        }

 

        private static ConcurrentDictionary<string, IDynamicEntityType> DynamicEntityTypes { get; } = new ConcurrentDictionary<string, IDynamicEntityType>();
        public static IDynamicEntityType GetOrAddDynamicEntityType(string entityType)
        {
            Contract.Requires( !string.IsNullOrEmpty(entityType));
            if (DynamicEntityTypes.ContainsKey(entityType)) return DynamicEntityTypes[entityType];
            using (var ctx = new GenSoftDBContext())
            {
                try
                {

                    var viewType = ctx.EntityTypes
                        .Include(x => x.Application.DatabaseInfo)
                        .Include(x => x.Type)
                        .Include(x => x.ParentEntityType.EntityType.Type)
                        .Include(x => x.ParentEntityType.EntityType.Application.DatabaseInfo)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperty.CalculatedPropertyParameters).ThenInclude(x => x.FunctionParameter)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperty.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedPropertyParameterEntityTypes).ThenInclude(x => x.EntityTypeAttribute.Attribute)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperty.FunctionParameterConstants)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperty.FunctionSet.FunctionSetFunctions).ThenInclude(x => x.Function)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.EntityTypeAttributeCach)
                        .Include(x => x.EntityTypeAddinActions).ThenInclude(x => x.AddinAction.Addin)
                        .FirstOrDefault(x => x.Type.Name == entityType);
                    if (viewType == null) return DynamicEntityType.NullEntityType();

                    
                    

                    var viewset = ctx.EntityTypeAttributes
                        .Where(x => x.EntityTypeId == viewType.Id)
                        .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                        .Select(x => x.AttributeId).ToList();

                   

                    if (!viewset.Any()) return DynamicEntityType.NullEntityType();
                    var tes =

                        ctx.Entities
                            .FirstOrDefault(x => x.Id == 0)
                            ?.EntityAttributes
                            .Where(z => viewset.Contains(z.AttributeId))
                            .OrderBy(d => viewset.IndexOf(d.AttributeId))
                            .Select(z => new EntityKeyValuePair(z.Attribute.Name, z.Value,
                                (ViewAttributeDisplayProperties)CreateEntityAttributeViewProperties(z.Id), false,
                                z.Attribute.EntityId != null, false) as IEntityKeyValuePair) // z.Entity.EntityType.EntityTypeAttributes.EntityName != null%/
                            .ToList()
                        ??
                        ctx.EntityTypeAttributes
                            .Where(x => x.EntityTypeId == viewType.Id)
                            .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                            .Select(z =>
                                new EntityKeyValuePair(z.Attribute.Name,
                                    null,
                                    (ViewAttributeDisplayProperties)CreateEntityAttributeViewProperties(z.Id), false,
                                    z.Attribute.EntityId != null,
                                    z.EntityName != null) as IEntityKeyValuePair).ToList();

                    
                    var calPropDef = CreateCalculatedProperties(viewType);

                    var cachedProperties = CreateCachedProperties(viewType);
                    var relationships = CreatePropertyParentEntityTypes(viewType);
                    var actions = CreateAddinActions(viewType);

                    IDynamicEntityType parentEntityType;
                    if (viewType.ParentEntityType == null)
                    {
                        parentEntityType = DynamicEntityType.NullEntityType();
                    }
                    else
                    {
                        parentEntityType = GetOrAddDynamicEntityType(viewType.ParentEntityType.EntityType.Type.Name);

                        CreateParentPropertyCache(tes, parentEntityType, viewType, cachedProperties);
                    }

                    foreach (var p in relationships.parentTypes)
                    {
                        CreateParentPropertyCache(tes, GetOrAddDynamicEntityType(p.Type), viewType, cachedProperties);
                    }
                    //Inherited Name
                    if (!cachedProperties.ContainsKey("Name") && viewType.ParentEntityType != null)
                    {
                        CreateInheritedEntityNamePropertyCache(tes, parentEntityType, viewType, cachedProperties);
                    }
                    //ComposedName
                    if (!cachedProperties.ContainsKey("Name") && relationships.parentTypes.Any())
                    {
                        CreateComposedEntityNamePropertyCache(relationships.parentTypes, viewType, cachedProperties);
                        // remove name in properties
                        var name = tes.FirstOrDefault(x => x.Key == "Name");
                        if(name != null)tes.Remove(name);
                    }
                    // hide computed fields in write view
                    foreach (var field in tes.Where(x => x.IsComputed))
                    {
                        field.DisplayProperties.WriteProperties.Properties["AttributeLabel"]["Visibility"] = "Collapsed";
                        field.DisplayProperties.WriteProperties.Properties["AttributeValue"]["Visibility"] = "Collapsed";
                    }

                    var dynamicEntityType = new DynamicEntityType(viewType.Type.Name,
                        viewType.EntitySet, tes, calPropDef, cachedProperties, relationships.parentTypes, relationships.childTypes, parentEntityType, actions);



                    DynamicEntityTypes.AddOrUpdate(entityType, dynamicEntityType);
                    return DynamicEntityTypes[entityType];
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static ObservableList<IAddinAction> CreateAddinActions(EntityType viewType)
        {
            var res = new List<IAddinAction>();
            foreach (var eAction in viewType.EntityTypeAddinActions)
            {
                var type = Utilities.TypeNameExtensions.GetTypeByName($"{eAction.AddinAction.Addin.Name}.{eAction.AddinAction.Class}").FirstOrDefault();
                var a = Activator.CreateInstance(type);
                var aa = new AddinAction(eAction.AddinAction.Class, eAction.AddinAction.Addin.Name, eAction.Name);
                res.Add(aa);
            }
            
           return new ObservableList<IAddinAction>(res);
        }

        private static void CreateParentPropertyCache(List<IEntityKeyValuePair> tes, IDynamicEntityType parentEntityType,
           EntityType viewType, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties)
        {

            if (parentEntityType.Properties.Any(x => x.Key == "Name"))
            {
                using (var ctx = new GenSoftDBContext())
                {
                    var pEntityType = ctx.EntityTypes
                        .Include(x => x.Application.DatabaseInfo)
                        .Include(x => x.ParentEntityType.EntityType.Type)
                        .Include(x => x.Type)
                        .First(x => x.Type.Name == parentEntityType.Name);

                    var pAtt = ctx.EntityTypeAttributes
                        .Include(x => x.Attribute)
                        .Where(x => x.EntityTypeId == pEntityType.Id &&
                                    x.Attribute.Name == "Name")
                        .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                        .FirstOrDefault();
                    if (pAtt == null) return;
                    
                        if (tes.All(x => x.Key != pAtt.Attribute.Name))
                            tes.Add(new EntityKeyValuePair(pAtt.Attribute.Name,
                                null,
                                (ViewAttributeDisplayProperties)CreateEntityAttributeViewProperties(pAtt.Id), true,
                                pAtt.Attribute.EntityId != null,
                                pAtt.EntityName != null) as IEntityKeyValuePair);
                        CreateCacheProperty(parentEntityType, viewType, cachedProperties, pEntityType, pAtt);
                    
                }
            }
        }

        private static void CreateComposedEntityNamePropertyCache(List<IDynamicRelationshipType> propertyParentEntityTypes,
            EntityType viewType, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties)
        {
            //get list of items
            if (viewType.Application.DatabaseInfo == null)
            {
                DynamicCreateComposedEntityNameCache(propertyParentEntityTypes, viewType, cachedProperties);
            }
            else
            {
                DBCreateComposedEntityNameCache(propertyParentEntityTypes, viewType, cachedProperties);
            }
            // Calculate the Name Property
        }

        private static void DynamicCreateComposedEntityNameCache(List<IDynamicRelationshipType> propertyParentEntityTypes, EntityType viewType, ObservableDictionary<string, Dictionary<int, object>> cachedProperties)
        {
            var propertyKey = "Name";
            using (var ctx = new GenSoftDBContext())
            {
                var entityTypeId = ctx.EntityTypes
                    .FirstOrDefault(x => x.Type.Name == viewType.Type.Name)?.Id;



                var viewEntityAttributes = ctx.EntityTypeAttributes.AsNoTracking()
                    .Where(x => x.EntityTypeId == entityTypeId)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .Select(x => x.AttributeId).ToList(); 

                var res =  ctx.Entities.AsNoTracking().Include(x => x.EntityAttributes)
                    .ThenInclude(x => x.Attribute).ThenInclude(x => x.EntityId)
                    .ThenInclude(x => x.Attribute)
                    .Where(x => x.EntityTypeId == entityTypeId)
                    .Where(x => x.EntityAttributes.Any(z => z.Attribute.EntityId != null));

                var viewset = res.Select(x => new {x.Id, Properties = 
                        x.EntityAttributes
                            .Where(z => viewEntityAttributes.Contains(z.AttributeId))
                            .OrderBy(d => viewEntityAttributes.IndexOf(d.AttributeId))
                            .Select(z => new { z.Attribute.Name, z.Value })
                            .ToDictionary(q => q.Name, q => q.Value as object)}).ToList();

                foreach (var itm in viewset)
                {
                   
                        var id = itm.Id;
                        
                        var val = propertyParentEntityTypes.Where(x => x.Key != "Id" && itm.Properties.ContainsKey(x.Key) && GetOrAddDynamicEntityType(x.Type).CachedProperties.ContainsKey("Name") && GetOrAddDynamicEntityType(x.Type).CachedProperties["Name"].ContainsKey(Convert.ToInt32(itm.Properties[x.Key])))//.OrderByDescending(x => x.Key)
                            .Select(x =>  GetOrAddDynamicEntityType(x.Type).CachedProperties["Name"][Convert.ToInt32(itm.Properties[x.Key])])
                            .Aggregate("", (c, n) => $"{c}-{n}");
                        if (cachedProperties.ContainsKey(propertyKey))
                        {
                            if (cachedProperties[propertyKey].ContainsKey(id)) return;
                            cachedProperties[propertyKey].Add(id, val);
                        }
                        else
                        {
                            cachedProperties.Add(propertyKey, new Dictionary<int, dynamic>() { { id, val } });
                        }
                    
                }

            }
        }

        private static void DBCreateComposedEntityNameCache(List<IDynamicRelationshipType> propertyParentEntityTypes, EntityType viewType,
            ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties)
        {
            var propertyKey = "Name";
            using (var conn = new SqlConnection(viewType.Application.DatabaseInfo.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();
                var sql = $"Select Id, {propertyParentEntityTypes.Where(x => x.Key != "Id").Select(x => x.Key).Aggregate("", (c, n) => $"{c},{n}")} From {viewType.Type.Name}";
                cmd.CommandText = Sql.CleanSql(sql);
                    
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var aDict = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    var id = Convert.ToInt32(aDict.FirstOrDefault(x => x.Key == "Id").Value);
                    var value = propertyParentEntityTypes//.OrderByDescending(x => x.Key)
                        .Select(x => GetOrAddDynamicEntityType(x.Type).CachedProperties["Name"][(int) aDict[x.Key]])
                        .Aggregate((c, n) => $"{c}-{n}");

                    if (string.IsNullOrEmpty(value)) continue;
                    if (cachedProperties.ContainsKey(propertyKey))
                    {
                        if (cachedProperties[propertyKey].ContainsKey(id)) return;
                        cachedProperties[propertyKey].Add(id, value);
                    }
                    else
                    {
                        cachedProperties.Add(propertyKey, new Dictionary<int, dynamic>() {{id, value}});
                    }
                }
                reader.Close();
            }
        }

        private static void CreateInheritedEntityNamePropertyCache(List<IEntityKeyValuePair> tes, IDynamicEntityType parentEntityType,
            EntityType viewType, ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties)
        {
            
          
                using (var ctx = new GenSoftDBContext())
                {
                    var pEntityType = ctx.EntityTypes
                        .Include(x => x.Application.DatabaseInfo)
                        .Include(x => x.ParentEntityType.EntityType.Type)
                        .Include(x => x.Type)
                        .First(x => x.Type.Name == parentEntityType.Name);

                    var pAtt = ctx.EntityTypeAttributes
                        .Include(x => x.Attribute)
                        .Where(x => x.EntityTypeId == pEntityType.Id &&
                                    x.Attribute.Name == "Name")
                        .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                        .FirstOrDefault();
                    if (pAtt == null)
                    {
                        if (pEntityType.ParentEntityType != null && GetOrAddDynamicEntityType(pEntityType.ParentEntityType.EntityType.Type.Name)
                            .CachedProperties.ContainsKey("Name"))
                        {
                            cachedProperties.Add("Name", GetOrAddDynamicEntityType(pEntityType.ParentEntityType.EntityType.Type.Name).CachedProperties["Name"]);
                        }
                        else
                        {
                            
                        }
                        
                    }
                    else
                    {
                        if (tes.All(x => x.Key != pAtt.Attribute.Name))
                        {
                            var name = new EntityKeyValuePair(pAtt.Attribute.Name,
                                null,
                                (ViewAttributeDisplayProperties) CreateEntityAttributeViewProperties(pAtt.Id), true,
                                pAtt.Attribute.EntityId != null,
                                pAtt.EntityName != null);
                        
                        tes.Add(name);
                        }
                        CreateCacheProperty(parentEntityType, viewType, cachedProperties, pEntityType, pAtt, "Name");
                   }
                }
           
        }

        private static void CreateCacheProperty(IDynamicEntityType parentEntityType, EntityType viewType,
            ObservableDictionary<string, Dictionary<int, dynamic>> cachedProperties, EntityType pEntityType, EntityTypeAttribute pAtt, string propertyName = null)
        {
            if (viewType.Application.DatabaseInfo == null)
            {
                DynamicCachePropertyForAttribute(pEntityType, pAtt,
                    cachedProperties, propertyName ?? parentEntityType.Name);
            }
            else
            {
                DBCreateCachePropertyForAttribute(pEntityType, pAtt,
                    cachedProperties, propertyName ?? parentEntityType.Name);
            }
        }

        private static ObservableDictionary<string, Dictionary<int, dynamic>> CreateCachedProperties(EntityType viewType)
        {
            var res = new ObservableDictionary<string, Dictionary<int, dynamic>>();


            var lst = viewType.EntityTypeAttributes
                .Where(x => x.EntityTypeAttributeCach != null || x.Attribute.Name == "Name").DistinctBy(x => x.Id).ToList();
            if (viewType.Application.DatabaseInfo == null)
            {
                foreach (var cp in lst)
                {
                    DynamicCachePropertyForAttribute(viewType, cp, res);
                }
            }
            else
            {


                foreach (var cp in lst)
                {
                    DBCreateCachePropertyForAttribute(viewType, cp, res);
                }
            }

            return res;
        }

        private static void DBCreateCachePropertyForAttribute(EntityType viewType, EntityTypeAttribute cp, ObservableDictionary<string, Dictionary<int, dynamic>> res, string propertyKey = null)
        {
            using (var conn = new SqlConnection(viewType.Application.DatabaseInfo.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();
                if (propertyKey == null) propertyKey = cp.Attribute.Name;
                cmd.CommandText = $"Select Id, {cp.Attribute.Name} From {viewType.EntitySet}";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var aDict = Enumerable.Range(0, reader.FieldCount)
                        .ToDictionary(reader.GetName, reader.GetValue);
                    var id = Convert.ToInt32(aDict.FirstOrDefault(x => x.Key == "Id").Value);
                    KeyValuePair<string, object>? att = aDict.FirstOrDefault(x => x.Key != "Id");
                    if (att == null) continue;
                    if (res.ContainsKey(propertyKey))
                    {
                        if(res[propertyKey].ContainsKey(id)) return; // probly done already
                        res[propertyKey].Add(id, att.Value.Value);
                    }
                    else
                    {
                        res.Add(propertyKey, new Dictionary<int, dynamic>() {{id, att.Value.Value}});
                    }
                }
                reader.Close();
            }
        }

        private static void DynamicCachePropertyForAttribute(EntityType viewType, EntityTypeAttribute cp,
            ObservableDictionary<string, Dictionary<int, dynamic>> res, string propertyKey = null)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var elst = ctx.EntityAttributes
                    .Where(x => (x.AttributeId == cp.AttributeId || x.Attribute.Name == "Id") &&
                                x.Entity.EntityTypeId == viewType.Id)
                    .Select(x => new {Key = x.Attribute.Name, Value = x.Value, EntityId = x.EntityId})
                    .GroupBy(x => x.EntityId).ToList();
                if (propertyKey == null) propertyKey = cp.Attribute.Name;
                foreach (var g in elst)
                {
                    var id = Convert.ToInt32(g.FirstOrDefault(x => x.Key == "Id").Value);
                    var att = g.FirstOrDefault(x => x.Key != "Id");
                    if (att == null) continue;
                    if (res.ContainsKey(propertyKey))
                    {
                        res[propertyKey].Add(id, att.Value);
                    }
                    else
                    {
                        res.Add(propertyKey, new Dictionary<int, dynamic>() {{id, att.Value}});
                    }
                }
            }
        }

        public static dynamic CreatePredicate(string body, List<PredicateParameter> predicateParameters)
        {
            try
            {

                var interpreter = new Interpreter();
                if (body.Contains("const") || body.Contains("param"))
                {
                    return "\"EntityParameter or constant not assigned to entity attribute\"";

                }
                var returnType = typeof(bool);
                var parameters = predicateParameters.Where(x => !x.Parameter.Name.Contains("Const"));
                var argType = parameters.Select(x => CreateTypesFromDbType(x.Parameter.DataTypeId)).ToList();
                argType.Add(returnType);
                var funcType = TypeNameExtensions.GetTypeByName($"Func`{argType.Count}").First(x => x.IsGenericTypeDefinition == true);
                var expType = funcType.MakeGenericType(argType.ToArray());
                var exp = typeof(Interpreter).GetMethod("ParseAsDelegate").MakeGenericMethod(expType)
                    .Invoke(interpreter, new object[] { body, parameters.Select(x => x.Parameter.Name).ToArray() });


                return exp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static (List<IDynamicRelationshipType> parentTypes, List<IDynamicRelationshipType> childTypes) CreatePropertyParentEntityTypes(EntityType viewType)
        {
            var parentTypes = new List<IDynamicRelationshipType>(); //new ObservableDictionary<string, string>();
            var childTypes = new List<IDynamicRelationshipType>();

            using (var ctx = new GenSoftDBContext())
            {
                var lst = ctx.EntityTypeAttributes
                    //.Include(x => x.Attribute)
                    //.Include(x => x.EntityType.Type)
                    //.Include(x => x.EntityRelationships).ThenInclude(x => x.ParentEntity).ThenInclude(x => x.EntityTypeAttribute.EntityType.Type)
                    //.Include(x => x.EntityRelationships).ThenInclude(x => x.ParentEntity)
                    .Where(x => x.EntityTypeId == viewType.Id )//&& !x.ParentEntities.Any()
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)///////////////////////////// ---- reverse because dictionary act like a stack
                    .Select(x => new { Attribute = x.Attribute.Name,
                                       ParentEntities = x.EntityRelationships.Select(z => z.ParentEntity.EntityTypeAttribute.EntityType.Type.Name).ToList(),
                                       ChildEntities = x.EntityRelationships.Select(z => z.EntityTypeAttribute.EntityType.Type.Name).ToList(),
                    }).ToList();

                foreach (var p in lst)
                {
                    var parentType = p.ParentEntities.FirstOrDefault();
                    var childType = p.ChildEntities.FirstOrDefault();
                    if (parentType != null) parentTypes.Add(new DynamicRelationshipType(parentType, p.Attribute));
                    if (childType != null) childTypes.Add(new DynamicRelationshipType(childType, p.Attribute));
                }

            }

            return (parentTypes, childTypes) ;
        }

        private static IViewAttributeDisplayProperties CreateEntityAttributeViewProperties(int entityTypeAttributeId)
        {
            return new ViewAttributeDisplayProperties(
                CreateEntityAttributeDisplayProperties(entityTypeAttributeId, "Read"),
                CreateEntityAttributeDisplayProperties(entityTypeAttributeId, "Write")
            );

        }

        private static AttributeDisplayProperties CreateEntityAttributeDisplayProperties(int entityTypeAttributeId, string viewType)
        {

            return new AttributeDisplayProperties(
                CreateEntityTypeAttributeDisplayProperties(entityTypeAttributeId, viewType)
            );


        }

        private static Dictionary<string, Dictionary<string, string>> CreateEntityTypeAttributeDisplayProperties(
            int entityTypeAttributeId, string viewType)
        {
            try
            {
                using (var ctx = new GenSoftDBContext())
                {
                    var basicTheme = ctx.ViewPropertyThemes
                        .Include(x => x.ViewPropertyValueOption)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType)
                        .Select(x => new
                        {
                            PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name,
                            ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                            Value = (string)null
                        }).GroupBy(x => x.PresentationPropertyName)
                        .Select(x => new { x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value) }).ToDictionary(x => x.Key, x => x.Value);

                    var entityAttributeTheme =
                        ctx.EntityTypePresentationProperties
                            .Include(x => x.PropertyValueOption.ViewPropertyValueOption)
                            .Include(x => x.PropertyValue)
                            .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                            .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                            .Where(x => x.ViewType.Name == viewType && x.EntityTypeAttributeId == entityTypeAttributeId)
                            .Select(x => new
                            {
                                PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType
                                    .Name,
                                ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                                Value = x.PropertyValue == null ? x.PropertyValueOption.ViewPropertyValueOption.Value : x.PropertyValue.Value
                            }).GroupBy(x => x.PresentationPropertyName)
                            .Select(x => new { x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value) })
                            .ToDictionary(x => x.Key, x => x.Value);

                    foreach (var vt in entityAttributeTheme)
                    {


                        if (!basicTheme.ContainsKey(vt.Key))
                        {

                            basicTheme.Add(vt.Key, vt.Value);
                        }
                        else
                        {
                            var bt = basicTheme[vt.Key];
                            foreach (var etitm in vt.Value)
                            {
                                if (bt.ContainsKey(etitm.Key))
                                {

                                    bt[etitm.Key] = etitm.Value;

                                }
                                else
                                {
                                    bt.Add(etitm.Key, etitm.Value);
                                }
                            }
                        }


                    }

                    return basicTheme;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        static Dictionary<int, Type> DBTypes = new Dictionary<int, Type>();
        private static Type CreateTypesFromDbType(int dataTypeId)
        {
            if (DBTypes.ContainsKey(dataTypeId)) return DBTypes[dataTypeId];

            var typedic = new Dictionary<Tuple<int, Type>, List<Type>>();

            using (var ctx = new GenSoftDBContext())
            {
                var dataType = ctx.DataTypes
                    .Include(x => x.Type.Types).ThenInclude(x => x.ChildType)
                    .Include(x => x.Type.Types).ThenInclude(x => x.ParentType)
                    .First(x => x.Id == dataTypeId);
                if (dataType.Type.Types.Count == 0)
                {


                    var res = TypeNameExtensions.GetTypeByName(dataType.Type.Name);
                    if (res == null) Debugger.Break();
                    DBTypes.Add(dataTypeId, res[0]);
                    return res[0];
                }
                else
                {

                    foreach (var typeArguements in dataType.Type.Types.DistinctBy(x => x.Id))
                    {
                        var parentType = new Tuple<int, Type>(typeArguements.ParentTypeId, CreateTypesFromDbType(typeArguements.ParentTypeId));
                        var childType = CreateTypesFromDbType(typeArguements.ChildTypeId);
                        if (typedic.ContainsKey(parentType))
                        {
                            typedic[parentType].Add(childType);
                        }
                        else
                        {
                            typedic.Add(parentType, new List<Type>() { childType });
                        }

                    }

                    if (typedic.Count > 1) Debugger.Break();

                    var t = typedic.First();
                    var res = t.Key.Item2 == typeof(Array)
                        ? t.Value[0].MakeArrayType()
                        : t.Key.Item2.MakeGenericType(t.Value.ToArray());

                    DBTypes.Add(dataType.Id, res);
                    return res;

                }
            }
        }

        public static dynamic CreateFunction(string body, List<FunctionParameter> FunctionParameter, int returnDataTypeId)
        {
            try
            {

                var interpreter = new Interpreter();
                if (body.Contains("const") || body.Contains("param"))
                {
                    return "\"EntityParameter or constant not assigned to entity attribute\"";

                }
                var returnType = CreateTypesFromDbType(returnDataTypeId);
                var parameters = FunctionParameter;
                var argType = parameters.Select(x => CreateTypesFromDbType(x.DataTypeId)).ToList();
                argType.Add(returnType);
                var funcType = TypeNameExtensions.GetTypeByName($"System.Func`{argType.Count}")
                    .First(x => x.IsGenericTypeDefinition == true);
                var expType = funcType.MakeGenericType(argType.ToArray());
                var exp = typeof(Interpreter).GetMethod("ParseAsDelegate").MakeGenericMethod(expType)
                    .Invoke(interpreter, new object[] { body, parameters.Select(x => x.Name).ToArray() });


                return exp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string CreateCPFunctionBody(string body, List<FunctionParameter> FunctionParameter, int calculatedPropertyId)
        {
            var paramlst = new Dictionary<string, string>();


            foreach (var p in FunctionParameter)
            {
                var cpList = p.CalculatedPropertyParameters.DistinctBy(x => x.Id)
                    .Where(x => x.CalculatedPropertyId == calculatedPropertyId).Select(x => x.CalculatedProperty).DistinctBy(x => x.Id).ToList();
                foreach (var cp in cpList)
                {
                    var pconst = cp.FunctionParameterConstants.DistinctBy(x => x.Id)
                        .Where(q => q.FunctionParameterId == p.Id).ToList();
                    for (int j = 0; j < pconst.Count(); j++)
                    {
                        var c = pconst[j];
                        paramlst.AddOrUpdate($"const{j}", $"\"{c.Value}\"");
                    }

                    var pEntityParameters =
                        cp.CalculatedPropertyParameters.Where(x => x.FunctionParameterId == p.Id)
                            .DistinctBy(x => x.Id).ToList();
                    for (int j = 0; j < pEntityParameters.Count(); j++)
                    {
                        var param = pEntityParameters[j];
                        var cparameter = param.CalculatedPropertyParameterEntityTypes.DistinctBy(x => x.Id)
                            .FirstOrDefault(x => x.CalculatedPropertyParameterId == param.Id)
                            ?.EntityTypeAttribute.Attribute.Name;

                        if (cparameter != null) paramlst.AddOrUpdate($"param{j}", $"\"{cparameter}\"");
                    }
                }
            }
            var newBody = paramlst.Aggregate(body, (current, p) => current.Replace(p.Key, p.Value));

            return newBody;
        }


        private static Dictionary<string, List<dynamic>> CreateCalculatedProperties(EntityType viewType)
        {

            var calprops = new Dictionary<string, List<dynamic>>();


            var lst = viewType.EntityTypeAttributes
                .Where(x => x.CalculatedProperty != null)
                .Select(x => x.CalculatedProperty).DistinctBy(x => x.Id).ToList();
            foreach (var cp in lst)
            {
                var flst = cp
                    .FunctionSet
                    .FunctionSetFunctions
                    .DistinctBy(x => x.Id)
                    .Select(f =>
                        Functions.ContainsKey(f.Function.Name)
                            ? Functions[f.Function.Name]
                            : CreateFunction(
                                CreateCPFunctionBody(
                                    f.Function.Body,
                                    f.Function.FunctionParameters.ToList(),
                                    cp.Id),
                                f.Function.FunctionParameters.ToList(),
                                f.Function.ReturnDataTypeId)).ToList();

                calprops.Add(cp.EntityTypeAttribute.Attribute.Name, flst);
            }


            return calprops;
        }

        public static void ResetDynamicTypes()
        {
            DynamicEntityTypes.Clear();
        }
    }
}
