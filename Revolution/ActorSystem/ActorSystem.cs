using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using BuildingSpecifications;
using Common;
using Common.DataEntites;
using Common.Dynamic;
using DataServices.Actors;
using DynamicExpresso;
using GenSoft.DBContexts;
using GenSoft.Entities;
using GenSoft.Expressions;
using JB.Collections.Reactive;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Process.WorkFlow;
using RevolutionData;
using RevolutionEntities.Process;
using Utilities;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;
using ViewModel.WorkFlow;
using Type = System.Type;

namespace ActorBackBone
{
    [Export(typeof(IActorBackBone))]
    public class ActorBackBone: IActorBackBone
    {
        //ToDo:Get rid of private setter
        public static ActorBackBone Instance { get; private set; }
       
        public static ActorSystem System { get; private set; }


        public void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<IProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos)
        {
             try
            {
                System = ActorSystem.Create("System");

                BuildExpressions();

                var res = GetDBComplexActions();
                res.AddRange(complexEventActions);

                var vmres = GetDBViewInfos();
                vmres.AddRange(viewInfos);

                System.ActorOf(Props.Create<ServiceManager>(autoRun,machineInfo, processInfos, res,vmres),"ServiceManager");
                Instance = this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BuildExpressions()
        {

            using (var ctx = new GenSoftDBContext())
            {
                var lst = ctx.Functions
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.FunctionParameterConstants)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.FunctionParameters)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedPropertyParameterEntityTypes).ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.FunctionParameterConstants)
                    .ToList();

                foreach (var f in lst)
                {
                    try
                    {
                        var interpreter = new Interpreter();
                        
                        
                        var body = f.Body;
                        var paramlst = new Dictionary<string, string>();

                        var functionParameters = f.FunctionParameters.DistinctBy(x => x.Id).ToList();
                        if (functionParameters.SelectMany(x => x.CalculatedPropertyParameters).Any() || functionParameters.SelectMany(x => x.FunctionParameterConstants).Any())
                        {
                            foreach (var p in functionParameters)
                            {
                                var cpList = p.CalculatedPropertyParameters.DistinctBy(x => x.Id).ToList();
                                foreach (var cp in cpList)
                                {
                                    var pconst = cp.CalculatedProperties.FunctionParameterConstants.DistinctBy(x => x.Id)
                                        .Where(q => q.FunctionParameterId == p.Id).ToList();
                                    for (int j = 0; j < pconst.Count; j++)
                                    {
                                        var c = pconst[j];
                                        paramlst.AddOrSet($"const{j}", $"\"{c.Value}\"");
                                    }

                                    var pEntityParameters =
                                        cp.CalculatedProperties.CalculatedPropertyParameters.Where(x => x.FunctionParameterId == p.Id).DistinctBy(x => x.Id).ToList();
                                    for (int j = 0; j < pEntityParameters.Count; j++)
                                    {
                                        var param = pEntityParameters[j];
                                        var cparameter = param.CalculatedPropertyParameterEntityTypes.DistinctBy(x => x.Id)
                                        .FirstOrDefault(x => x.CalculatedPropertyParameterId == param.Id)
                                        ?.EntityTypeAttributes.Attributes.Name;

                                        if(cparameter!= null) paramlst.AddOrSet($"param{j}", $"\"{cparameter}\"");
                                    }
                                    

                                }
                            }
                            body = paramlst.Aggregate(body, (current, p) => current.Replace(p.Key, p.Value));
                        }

                        
                        var returnType = CreateTypesFromDbType(f.ReturnDataTypeId);
                        var parameters = functionParameters;
                        var argType = parameters.Select(x => CreateTypesFromDbType(x.DataTypeId)).ToList();
                        argType.Add(returnType);
                        var funcType = GetTypeByName($"Func`{argType.Count}")
                            .First(x => x.IsGenericTypeDefinition == true);
                        var expType = funcType.MakeGenericType(argType.ToArray());
                        var exp = typeof(Interpreter).GetMethod("ParseAsDelegate").MakeGenericMethod(expType)
                            .Invoke(interpreter, new object[] {body, parameters.Select(x => x.Name).ToArray()});

                         

                        DynamicEntityType.Functions.Add(f.Name, exp);



                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }



        }



        static Dictionary<int, Type> DBTypes = new Dictionary<int, Type>();
        private static Type CreateTypesFromDbType(int dataTypeId)
        {
            if (DBTypes.ContainsKey(dataTypeId)) return DBTypes[dataTypeId];

            var typedic = new Dictionary<Tuple<int,Type>, List<Type>>();
            
            using (var ctx = new GenSoftDBContext())
            {
                var dataType = ctx.DataType
                                .Include(x => x.Type.Types).ThenInclude(x => x.ChildType)
                                .Include(x => x.Type.Types).ThenInclude(x => x.ParentType)
                    .First(x => x.Id == dataTypeId);
                if (dataType.Type.Types.Count == 0)
                {

                    
                    var res = GetTypeByName(dataType.Type.Name);
                    if(res == null) Debugger.Break();
                    DBTypes.Add(dataTypeId, res[0]);
                    return res[0];
                }
                else
                {
                    
                    foreach (var typeArguements in dataType.Type.Types.DistinctBy(x => x.Id))
                    {
                        var parentType = new Tuple<int, Type>(typeArguements.ParentTypeId,CreateTypesFromDbType(typeArguements.ParentTypeId));
                        var childType = CreateTypesFromDbType(typeArguements.ChildTypeId);
                        if (typedic.ContainsKey(parentType))
                        {
                            typedic[parentType].Add(childType);
                        }
                        else
                        {
                            typedic.Add(parentType, new List<Type>(){childType});
                        }
                            
                    }

                    if(typedic.Count > 1) Debugger.Break();

                    var t = typedic.First();
                    var res = t.Key.Item2 == typeof(Array) 
                        ? t.Value[0].MakeArrayType() 
                        : t.Key.Item2.MakeGenericType(t.Value.ToArray());
                    
                    DBTypes.Add(dataType.Id, res);
                    return res;

                }
            }
        }

        public static Type[] GetTypeByName(string className)
        {
            var returnVal = new List<Type>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyTypes = a.GetTypes();
                returnVal.AddRange(assemblyTypes.Where(t => t.Name.ToLower() == className.ToLower() ));//|| t.FullName.ToLower().Contains(className.ToLower()) 
            }

            return returnVal.ToArray();
        }

        private List<IViewModelInfo> GetDBViewInfos()
        {
            var res = new List<IViewModelInfo>();
            using (var ctx = new GenSoftDBContext())
            {
                foreach (var r in ctx.DomainEntityType
                    .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.EntityTypeViewModel)
                    .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.ProcessState.Process)
                    
                    .Where(x => 
                                (!x.EntityType.EntityTypeAttributes.SelectMany(z => z.ChildEntitys).Any() )) //&& x.EntityType.EntityTypeAttributes.SelectMany(z => z.ParentEntitys).Any()
                    .OrderBy(x => x.Id)
                )
                {
                    
                    foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id)
                        .Distinct())
                    {
                        
                        foreach (var pd in r.ProcessStateDomainEntityTypes.Where(
                            x => x.ProcessState.ProcessId == processId).DistinctBy(x => x.Id))
                        {
                            if (pd.EntityTypeViewModel.Any())
                            {
                                foreach (var vm in pd.EntityTypeViewModel.DistinctBy(x => x.Id))
                                {
                                    res.Add(CreateEntityViewModel(ctx, vm.Id, res));
                                }
                            }
                            else
                            {
                                DoChildViews(ctx, pd.DomainEntityTypeId, res);
                            }

                        }

                    }
                    if(!r.ProcessStateDomainEntityTypes.Any()) DoChildViews(ctx, r.Id, res);
                }
            }
            return res;
        }

        private static void DoChildViews(GenSoftDBContext ctx, int domainEntityTypeId, List<IViewModelInfo> res)
        {
            var childviewModels = ctx.EntityRelationships
                .Include(x => x.ChildEntity.ChildEntitys)
                .Where(x => x.ParentEntity.EntityTypeId == domainEntityTypeId)
                .SelectMany(x => x.ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes)
                .SelectMany(x => x.EntityTypeViewModel).Select(x => x.Id);
            foreach (var c in childviewModels)
            {
                res.Add(CreateEntityViewModel(ctx, c, res));
            }
        }

        private static IViewModelInfo CreateEntityViewModel(GenSoftDBContext ctx, int vmId, List<IViewModelInfo> viewInfos, bool isChild = false)
        {
            var vm = ctx.EntityTypeViewModel
                .Include(x => x.EntityViewModelCommands).ThenInclude(x => x.ViewModelCommands.CommandTypes)
                .Include(x => x.ProcessStateDomainEntityTypes).ThenInclude(x => x.ProcessState.Process)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityTypeAttributes).ThenInclude(x => x.EntityType.DomainEntityType)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityTypeAttributes).ThenInclude(x => x.ChildEntitys).ThenInclude(x => x.ParentEntity.EntityType.Type)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityTypeAttributes).ThenInclude(x => x.ChildEntitys).ThenInclude(x => x.ParentEntity.Attributes)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityTypeAttributes)
                        .ThenInclude(x => x.ParentEntitys).ThenInclude(x => x.ChildEntity.EntityType.Type)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityTypeAttributes)
                        .ThenInclude(x => x.ParentEntitys).ThenInclude(x => x.ChildEntity.Attributes)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityView)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.DomainEntityType.DomainEntityCache)
                .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityList)
                .Include(x => x.ViewModelTypes.ViewModelPropertyPresentationType).ThenInclude(x => x.PresentationTheme)
                .Include(x => x.ViewModelTypes.ViewModelPropertyPresentationType).ThenInclude(x => x.ViewPropertyValueOptions)
                .Include(x => x.ViewModelTypes.ViewModelPropertyPresentationType).ThenInclude(x => x.ViewType)
                .Include(x => x.ViewModelTypes.ViewModelPropertyPresentationType).ThenInclude(x => x.ViewPropertyPresentationPropertyType)
                .First(x => x.Id == vmId);
            //var viewModelTypeName =
            //    ctx.ViewModelTypes.First(
            //        x => x.List == (vm.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityList != null) &&
            //             x.DomainEntity == true).Name;
            var vp = CreateViewAttributeDisplayProperties(vm);


            var res = ProcessViewModels.ProcessViewModelFactory[vm.ViewModelTypes.Name].Invoke(vm,vp);


            

            var childviewModels = ctx.EntityRelationships
                .Include(x => x.ChildEntity.ChildEntitys)
                .Where(x => x.ParentEntity.EntityTypeId == vm.ProcessStateDomainEntityTypes.DomainEntityTypeId)
                .SelectMany(x => x.ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes)
                .SelectMany(x => x.EntityTypeViewModel).Select(x => x.Id);

            foreach (var cvm in childviewModels)
            {
                var v = CreateEntityViewModel(ctx, cvm,viewInfos, true);
                if (!isChild && vm.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.EntityList != null)
                {
                    viewInfos.Add(v);
                }
                else
                {
                    res.ViewModelInfos.Add(v);
                }
                    
                }
            
            return res;
        }

        private static ViewAttributeDisplayProperties CreateViewAttributeDisplayProperties(EntityTypeViewModel vm)
        {
            return new ViewAttributeDisplayProperties
                (
                CreateAttributeDisplayProperties(vm, "Read"),
                CreateAttributeDisplayProperties(vm, "Write")
                );
        }

        private static AttributeDisplayProperties CreateAttributeDisplayProperties(EntityTypeViewModel vm, string viewType)
        {
            return new AttributeDisplayProperties(CreateDisplayProperties(vm, viewType));
        }

        private static Dictionary<string, Dictionary<string, string>> CreateDisplayProperties(EntityTypeViewModel vm, string viewType)
        {
            using (var ctx = new GenSoftDBContext())
            {

                var basicTheme = ctx.ViewPropertyTheme
                    .Include(x => x.ViewPropertyValueOptions)
                    .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                    .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                    .Where(x => x.ViewType.Name == viewType)
                    //.GroupBy(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name)
                    .Select(x => new
                    {
                        PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name,
                        ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                        x.ViewPropertyValueOptions.Value
                    }).GroupBy(x => x.PresentationPropertyName)
                    .Select(x => new {x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value)}).ToDictionary(x => x.Key, x => x.Value);

                var viewTypeTheme =
                    ctx.ViewModelPropertyPresentationType
                        .Include(x => x.ViewPropertyValueOptions)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType && x.ViewModelTypeId == vm.ViewModelTypeId)
                        //.GroupBy(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name)
                        .Select(x => new
                        {
                            PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name,
                            ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                            x.ViewPropertyValueOptions.Value
                        }).GroupBy(x => x.PresentationPropertyName)
                        .Select(x => new { x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value) }).ToDictionary(x => x.Key, x => x.Value);



                foreach (var vt in viewTypeTheme)
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

                var displayProperties = basicTheme;
               return displayProperties;

            }
            
        }

        private List<IComplexEventAction> GetDBComplexActions()
        {
            try
            {





                var res = new List<IComplexEventAction>();
                using (var ctx = new GenSoftDBContext())
                {
                    foreach (var r in ctx.DomainEntityType
                        .Include(x => x.DomainEntityTypeSourceEntity)
                        .Include(x => x.EntityType.Type)
                        .Include(x => x.EntityType.EntityView)
                        .Include(x => x.EntityType.DomainEntityType.DomainEntityCache)
                        .Include(x => x.EntityType.EntityList)
                        .Include("ProcessStateDomainEntityTypes.ProcessState.Process")
                        .OrderBy(x => x.Id)
                        )
                    {
                        var entityType = r.EntityType.Type.Name;


                       if(! AddDynamicEntityTypes(ctx, entityType)) continue;
                        
                        foreach (var processId in r.ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            

                            if (r.EntityType.EntityView != null)
                            {
                                if (r.EntityType.EntityList == null)
                                {

                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateState", new object[] {processId,DynamicEntityType.DynamicEntityTypes[entityType]}));
                                }
                                else
                                {
                                    if(r.EntityType.EntityList.Initalize) res.Add(Processes.ComplexActions.GetComplexAction("IntializeProcessState", new object[] { processId, DynamicEntityType.DynamicEntityTypes[entityType] }));
                                    res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList", new object[] {processId, DynamicEntityType.DynamicEntityTypes[entityType] }));
                                }
                            }

                            if(r.EntityType.DomainEntityType.DomainEntityCache != null )
                                res.Add(EntityComplexActions.GetComplexAction("IntializeCache",  new object[] { processId, DynamicEntityType.DynamicEntityTypes[entityType] }));

                            
                        }
                    }

                    foreach (var r in ctx.EntityRelationships
                        .Include(x => x.ChildEntity.EntityType.Type)
                        .Include(x => x.ChildEntity.Attributes)
                        .Include(x => x.ChildEntity.EntityType.EntityList)
                        .Include(x => x.ParentEntity.EntityType.Type)
                        .Include(x => x.ParentEntity.Attributes)
                        .Include(x => x.ParentEntity.EntityType.EntityList)
                        .Include(x => x.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity)
                        .Include(
                            "ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                        .OrderBy(x => x.ParentEntity.Id)
                        .Where(x => x.ChildEntity.EntityType.CompositeRequest == null))
                    {
                        var parentType = r.ParentEntity.EntityType.Type.Name;
                        var childType = r.ChildEntity.EntityType.Type.Name;
                        var sourceEntityName = r.ChildEntity.EntityType.DomainEntityType.DomainEntityTypeSourceEntity
                            ?.SourceEntity;

                        var parentExpression = r.ParentEntity.Attributes.Name;

                        var childExpression = r.ChildEntity.Attributes.Name;

                        foreach (var processId in r.ChildEntity.EntityType.DomainEntityType
                            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {

                            if (r.ChildEntity.EntityType.EntityList == null)
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestState",
                                    new object[] {processId, parentType, childType, childExpression}));
                            }
                            else
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("RequestStateList",new object[] {processId,parentType, childType, parentExpression, childExpression}));
                            }
                            res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",new object[] {processId,parentType, childType, parentExpression, childExpression}));
                        }

                    }


                    foreach (var g in ctx.EntityRelationships
                        .Include("ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                        .Where(x => x.ChildEntity.EntityType.CompositeRequest != null)
                        .GroupBy(x => x.ChildEntity.EntityType).Where(g => g.Count() > 1))                        
                    {

                        foreach (var processId in g.Key.DomainEntityType
                            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                        {
                            var childType = g.Key.Type.Name;
                            var parentEntities = g.Select(p =>
                                                new ViewModelEntity() {
                                                    EntityType = DynamicEntityType.DynamicEntityTypes[p.ParentEntity.EntityType.Type.Name],
                                                    Property = p.ChildEntity.Attributes.Name}
                                                    ).ToList();

                            res.Add(Processes.ComplexActions.GetComplexAction("RequestCompositeStateList",
                                new object[] { processId, DynamicEntityType.DynamicEntityTypes[childType], new Dictionary<string, dynamic>(), parentEntities }));
                        }
                    }


                    }
                return res;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        private static bool AddDynamicEntityTypes(GenSoftDBContext ctx, string entityType)
        {
            var viewType = ctx.EntityView
                .Include(x => x.EntityType.Type)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.Attributes)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.ChildEntitys)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperties.CalculatedPropertyParameters)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperties.FunctionParameterConstants)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.CalculatedProperties.FunctionSets.FunctionSetFunctions).ThenInclude(x => x.Functions)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                .Include(x => x.EntityType.EntityTypeAttributes).ThenInclude(x => x.EntityTypeAttributeCache)
                .Include(x => x.EntityType.EntityList)
                .FirstOrDefault(x => x.EntityType.Type.Name == entityType);
            if (viewType == null) return false;

            var viewset = ctx.EntityTypeAttributes
                .Where(x => x.EntityTypeId == viewType.Id)
                .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                .Select(x => x.AttributeId).ToList();

            if (!viewset.Any()) return false;
            var tes =

                ctx.Entity
                    .FirstOrDefault(x => x.Id == 0 && x.EntityTypeId == viewType.BaseEntityTypeId)
                    ?.EntityAttribute
                    .Where(z => viewset.Contains(z.AttributeId))
                    .OrderBy(d => viewset.IndexOf(d.AttributeId))
                    .Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value,
                       (ViewAttributeDisplayProperties) CreateEntityAttributeViewProperties(z.Id),
                        z.Attributes.EntityId != null, z.Attributes.EntityName != null) as IEntityKeyValuePair)
                    .ToList()
                ??
                ctx.EntityTypeAttributes
                    .Where(x => x.EntityTypeId == viewType.Id)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .Select(z =>
                        new EntityKeyValuePair(z.Attributes.Name,
                            null,
                            (ViewAttributeDisplayProperties) CreateEntityAttributeViewProperties(z.Id),
                            z.Attributes.EntityId != null,
                            z.Attributes.EntityName != null) as IEntityKeyValuePair).ToList();

            var calPropDef = CreateCalculatedProperties(viewType);

            var cachedProperties = CreateCachedProperties(viewType);
            
            var dynamicEntityType = new DynamicEntityType(viewType.EntityType.Type.Name, viewType.EntityType.EntitySetName, tes, calPropDef,cachedProperties, viewType.EntityType.EntityList != null, viewType.EntityType.EntityTypeAttributes.Any(z => z.ChildEntitys.Any()));



            DynamicEntityType.DynamicEntityTypes.Add(entityType,dynamicEntityType);
            return true;
        }

        private static ObservableDictionary<string, List<dynamic>> CreateCachedProperties(EntityView viewType)
        {
            var res = new ObservableDictionary<string, List<dynamic>>();
            

            var lst = viewType.EntityType.EntityTypeAttributes
                .Where(x => x.EntityTypeAttributeCache != null).DistinctBy(x => x.Id).ToList();


            using (var ctx = new GenSoftDBContext())
            {
                foreach (var cp in lst)
                {
                    var elst = ctx.EntityAttribute
                        .Where(x => x.AttributeId == cp.AttributeId && x.Entity.EntityTypeId == viewType.BaseEntityTypeId)
                        .Select(x => new {Key = x.Attributes.Name, Value = x.Value})
                        .GroupBy(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.Select(z => (dynamic)z.Value).ToList());
                    foreach (var itm in elst)
                    {
                         res.Add(itm.Key, itm.Value);
                    }
                   
                }
            }


            return res;
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
            using (var ctx = new GenSoftDBContext())
            {
                var basicTheme = ctx.ViewPropertyTheme
                    .Include(x => x.ViewPropertyValueOptions)
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
                    ctx.EntityViewModelPresentationProperties
                        .Include(x => x.ViewPropertyValueOptions)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType && x.EntityTypeViewModelAttributes.EntityTypeAttributeId == entityTypeAttributeId)
                        .Select(x => new
                        {
                            PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType
                                .Name,
                            ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                            x.ViewPropertyValueOptions.Value
                        }).GroupBy(x => x.PresentationPropertyName)
                        .Select(x => new {x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value)})
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


        private static Dictionary<string, List<dynamic>> CreateCalculatedProperties(EntityView viewType)
        {

            var calprops = new Dictionary<string, List<dynamic>>();

            
                var lst = viewType.EntityType.EntityTypeAttributes
                    .Where(x => x.CalculatedProperties != null)
                    .Select(x => x.CalculatedProperties).DistinctBy(x => x.Id).ToList();
                foreach (var cp in lst)
                {
                   var flst = cp.FunctionSets.FunctionSetFunctions.DistinctBy(x => x.Id).Select(f => DynamicEntityType.Functions[f.Functions.Name]).ToList();

                    calprops.Add(cp.EntityTypeAttributes.Attributes.Name, flst);
                }
            

                return calprops;
        }

        //private static List<IEntityKeyValuePair> GetCalculatedProperties(EntityView viewType, Dictionary<string, List<dynamic>> calPropDefinition)
        //{
        //    using (var ctx = new GenSoftDBContext())
        //    {
        //        var res = viewType.EntityType.EntityTypeAttributes
        //            .Where(x => x.CalculatedProperties != null)
        //            .Select(x => x.CalculatedProperties).DistinctBy(x => x.Id)
        //            .Select(x => new EntityKeyValuePair(x.EntityTypeAttributes.Attributes.Name,
        //               null,
        //                new ViewAttributeDisplayProperties
        //                (
        //                    CreateAttributeDisplayProperties(ctx, x.EntityTypeAttributes, "Read"),
        //                    CreateAttributeDisplayProperties(ctx, x.EntityTypeAttributes, "Write")
        //                ),
        //                viewType.EntityType.EntityList != null,
        //                viewType.EntityType.EntityTypeAttributes.Any(z => z.ChildEntitys.Any())
        //            ) as IEntityKeyValuePair).ToList();
        //        return res;
        //    }
            
        //}

        public void Intialize(bool autoContinue, List<IViewModelInfo> viewInfos)
        {
            using (var ctx = new GenSoftDBContext())
            {
                var machineInfo = ctx.Machine.Select(x => ProcessExpressions.CreateMachineInfo(x)).ToList();

                var processInfos = ctx.Process.Select(x => ProcessExpressions.CreateProcessInfo(x)).ToList();
                List<IComplexEventAction> dbComplexAction = new List<IComplexEventAction>(); 

                Intialize(autoContinue, machineInfo, processInfos, dbComplexAction, viewInfos);
            }

            
        }
    }
}
