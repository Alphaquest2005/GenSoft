using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using Common;
using Common.DataEntites;
using DynamicExpresso;
using EventAggregator;
using EventMessages.Commands;
using EventMessages.Events;
using GenSoft.DBContexts;
using GenSoft.Entities;
using GenSoft.Interfaces;
using JB.Collections.Reactive;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Process.WorkFlow;
using RevolutionData;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModel.WorkFlow;
using ISystemProcess = SystemInterfaces.ISystemProcess;
using IUser = SystemInterfaces.IUser;
using Process = System.Diagnostics.Process;
using SystemProcess = RevolutionEntities.Process.SystemProcess;
using Type = System.Type;


namespace DataServices.Actors
{
    public class DomainProcessSupervisor : BaseSupervisor<DomainProcessSupervisor>
    {
        private IUntypedActorContext ctx = null;

        //TODO: Track Actor Shutdown instead of just broadcast

        private List<SystemProcess> SystemProcess { get; } = new List<SystemProcess>();
        private List<DomainProcess> DomainProcess { get;  } = new List<DomainProcess>();

        public List<IComplexEventAction> ProcessComplexEvents { get; } = new List<IComplexEventAction>();
        public List<IViewModelInfo> ProcessViewModelInfos { get; } = new List<IViewModelInfo>();

        static int maxProcessId = 0;

        public DomainProcessSupervisor(bool autoRun, ISystemProcess process) : base(process)
        {
            
            
            BuildExpressions();
            List<EntityType> mainEntities;
            DomainProcess domainProcess;
            SystemProcess systemProcess;
            using (var ctx = new GenSoftDBContext())
            {
                maxProcessId = ctx.SystemProcess.Max(x => x.Id);

                domainProcess = ctx.DomainProcess
                        .Include(x => x.SystemProcess)
                        .Include(x => x.DomainProcessMainEntity).ThenInclude(x => x.EntityType)
                        .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                        .FirstOrDefault();
                if (domainProcess == null)
                {
                    maxProcessId += 1;
                    
                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, process.Id, "AutoSystemProcess",
                            "AutoSystemProcess", "Auto", process.User.UserId), process.User, process.MachineInfo);
                    SystemProcess.Add(
                        systemProcess);

                    domainProcess = new DomainProcess()
                    {
                        SystemProcess =
                            new GenSoft.Entities.SystemProcess() {Name = systemProcess.Name, Id = systemProcess.Id},
                        Id = systemProcess.Id
                    };
                    DomainProcess.Add(domainProcess);

                    var parentEntityTypes = ctx.EntityRelationship
                        .Select(x => x.ParentEntity.EntityTypeAttributes.EntityType).Distinct().ToList();
                    var childEntityTypes = ctx.EntityRelationship
                        .Select(x => x.EntityTypeAttributes.EntityType).Distinct().ToList();
                    mainEntities = parentEntityTypes.Where(z => !childEntityTypes.Contains(z)).ToList();

                    

                    foreach (var mainEntity in mainEntities)
                    {

                        ProcessComplexEvents.AddRange(GetDBComplexActions(mainEntity.Id, domainProcess.Id));
                        ProcessViewModelInfos.AddRange(GetDBViewInfos(mainEntity.Id, true, domainProcess.Id));

                    }

                   
                }
                else
                {
                    DomainProcess.Add(domainProcess);

                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(domainProcess.Id, domainProcess.SystemProcess.ParentProcessId, domainProcess.SystemProcess.Name,
                            domainProcess.SystemProcess.Description, domainProcess.SystemProcess.Symbol, process.User.UserId), process.User, process.MachineInfo);
                    SystemProcess.Add(systemProcess);
                    foreach (var mainEntity in domainProcess.DomainProcessMainEntity)
                    {
                        ProcessComplexEvents.AddRange(GetDBComplexActions(mainEntity.EntityType.Id, domainProcess.Id));
                        ProcessViewModelInfos.AddRange(GetDBViewInfos(mainEntity.EntityType.Id, false, domainProcess.Id));
                    }
                    
                }

            }
            
            EventMessageBus.Current.GetEvent<IMainEntityChanged>(Source).Subscribe(x => OnMainEntityChanged(x));
           

            ctx = Context;
          
          
            ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("StartProcess", new object[] { systemProcess.Id }));
            ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("CleanUpParentProcess", new object[] { systemProcess.Id, systemProcess.Id + 1}));
           // ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("CleanUpProcess", new object[] { systemProcess.Id, systemProcess.Id + 1 }));

            CreateProcesses(domainProcess, systemProcess);


        }


        private void OnMainEntityChanged(IMainEntityChanged mainEntityChanged)
        {
            ProcessComplexEvents.Clear();
            ProcessViewModelInfos.Clear();
            using (var ctx = new GenSoftDBContext())
            {
                 var entityType = ctx.EntityType.First(x => x.Type.Name == mainEntityChanged.EntityType.Name);
                 var domainProcess = ctx.DomainProcess
                    .Include(x => x.SystemProcess)
                    .Include(x => x.DomainProcessMainEntity).ThenInclude(x => x.EntityType)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .FirstOrDefault(x => x.DomainProcessMainEntity.Any(z => z.EntityType == entityType));
                SystemProcess systemProcess;

                maxProcessId += 1;
                if (domainProcess == null)
                {

                    
                    
                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, mainEntityChanged.Process.Id, $"AutoProcess-{entityType.EntitySetName}",
                            $"AutoProcess-{entityType.EntitySetName}", "Auto", mainEntityChanged.User.UserId), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                    

                    domainProcess = new DomainProcess()
                    {
                        SystemProcess = new GenSoft.Entities.SystemProcess() { Name = systemProcess.Name,
                            Id = systemProcess.Id,
                            ParentProcessId = systemProcess.ParentProcessId,
                            Description = systemProcess.Description,
                            Symbol = systemProcess.Symbol
                        },Id = systemProcess.Id
                    };
                    
                }
                else
                {
                    systemProcess = new SystemProcess(
                    new SystemProcessInfo(maxProcessId, mainEntityChanged.Process.Id, domainProcess.SystemProcess.Name,
                        domainProcess.SystemProcess.Description, domainProcess.SystemProcess.Symbol, mainEntityChanged.User.UserId), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                    domainProcess = new DomainProcess()
                    {
                        SystemProcess = new GenSoft.Entities.SystemProcess()
                        {
                            Name = systemProcess.Name,
                            Id = systemProcess.Id,
                            ParentProcessId = systemProcess.ParentProcessId,
                            Description = systemProcess.Description,
                            Symbol = systemProcess.Symbol
                        },
                        Id = systemProcess.Id
                    };

                }
                SystemProcess.Add(systemProcess);
                DomainProcess.Add(domainProcess);


                ProcessComplexEvents.AddRange(GetDBComplexActions(entityType.Id, systemProcess.Id));
                ProcessViewModelInfos.AddRange(GetDBViewInfos((int)entityType.Id, false, systemProcess.Id));

                ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("StartProcess", new object[] { systemProcess.Id }));
                ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("CleanUpParentProcess", new object[] {systemProcess.Id , systemProcess.Id + 1}));
                // ProcessComplexEvents.Add(Processes.ComplexActions.GetComplexAction("CleanUpProcess", new object[] { systemProcess.Id, systemProcess.Id + 1 }));

                CreateProcesses(domainProcess, systemProcess);


            }
        }

      

        
        
        private void CreateProcesses(DomainProcess domainProcess,SystemProcess systemProcess)
        {
            if (ProcessComplexEvents.All(x => x.ProcessId != domainProcess.Id))
                throw new ApplicationException(
                    $"No Complex Events were created for this process:{domainProcess.SystemProcess.Name}");


            
            var inMsg = new LoadDomainProcess(domainProcess,ProcessComplexEvents.Where(x => x.ProcessId == domainProcess.Id).ToList(),
                ProcessViewModelInfos.Where(x => x.ProcessId == domainProcess.Id).ToList(),
                new StateCommandInfo(systemProcess.Id, RevolutionData.Context.Process.Commands.StartProcess),
                systemProcess, Source);

            EventMessageBus.Current.Publish(inMsg, Source);


        }




        private void BuildExpressions()
        {

            using (var ctx = new GenSoftDBContext())
            {

                var lst = ctx.Functions
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.FunctionParameterConstant)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.FunctionParameter)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedPropertyParameterEntityTypes).ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters).ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.FunctionParameterConstant)
                    .Where(x => x.FunctionParameter.All(z => !z.CalculatedPropertyParameters.Any() && !z.FunctionParameterConstant.Any()))
                    .ToList();

                foreach (var f in lst)
                {

                    var FunctionParameter = f.FunctionParameter
                        .DistinctBy(x => x.Id).ToList();

                    var res = CreateFunction(f.Body, FunctionParameter, f.ReturnDataTypeId);
                    DynamicEntityType.Functions.Add(f.Name, res);
                }
            }



        }



        private static dynamic CreateFunction(string body, List<FunctionParameter> FunctionParameter, int returnDataTypeId)
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
                var funcType = GetTypeByName($"Func`{argType.Count}")
                    .First(x => x.IsGenericTypeDefinition == true);
                var expType = funcType.MakeGenericType(argType.ToArray());
                var exp = typeof(Interpreter).GetMethod("ParseAsDelegate").MakeGenericMethod(expType)
                    .Invoke(interpreter, new object[] { body, parameters.Select(x => x.Name).ToArray() });


                return exp;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static string CreateCPFunctionBody(string body, List<FunctionParameter> FunctionParameter, int calculatedPropertyId)
        {
            var paramlst = new Dictionary<string, string>();


            foreach (var p in FunctionParameter)
            {
                var cpList = p.CalculatedPropertyParameters.DistinctBy(x => x.Id)
                    .Where(x => x.CalculatedPropertyId == calculatedPropertyId).Select(x => x.CalculatedProperties).DistinctBy(x => x.Id).ToList();
                foreach (var cp in cpList)
                {
                    var pconst = cp.FunctionParameterConstant.DistinctBy(x => x.Id)
                        .Where(q => q.FunctionParameterId == p.Id).ToList();
                    for (int j = 0; j < pconst.Count(); j++)
                    {
                        var c = pconst[j];
                        paramlst.AddOrSet($"const{j}", $"\"{c.Value}\"");
                    }

                    var pEntityParameters =
                        cp.CalculatedPropertyParameters.Where(x => x.FunctionParameterId == p.Id)
                            .DistinctBy(x => x.Id).ToList();
                    for (int j = 0; j < pEntityParameters.Count(); j++)
                    {
                        var param = pEntityParameters[j];
                        var cparameter = param.CalculatedPropertyParameterEntityTypes.DistinctBy(x => x.Id)
                            .FirstOrDefault(x => x.CalculatedPropertyParameterId == param.Id)
                            ?.EntityTypeAttributes.Attributes.Name;

                        if (cparameter != null) paramlst.AddOrSet($"param{j}", $"\"{cparameter}\"");
                    }
                }
            }
            var newBody = paramlst.Aggregate(body, (current, p) => current.Replace(p.Key, p.Value));

            return newBody;
        }


        static Dictionary<int, Type> DBTypes = new Dictionary<int, Type>();
        private static Type CreateTypesFromDbType(int dataTypeId)
        {
            if (DBTypes.ContainsKey(dataTypeId)) return DBTypes[dataTypeId];

            var typedic = new Dictionary<Tuple<int, Type>, List<Type>>();

            using (var ctx = new GenSoftDBContext())
            {
                var dataType = ctx.DataType
                    .Include(x => x.Type.Types).ThenInclude(x => x.ChildTypes)
                    .Include(x => x.Type.Types).ThenInclude(x => x.ParentTypes)
                    .First(x => x.Id == dataTypeId);
                if (dataType.Type.Types.Count == 0)
                {


                    var res = GetTypeByName(dataType.Type.Name);
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

        public static Type[] GetTypeByName(string className)
        {
            var returnVal = new List<Type>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyTypes = a.GetTypes();
                returnVal.AddRange(assemblyTypes.Where(t => t.Name.ToLower() == className.ToLower()));//|| t.FullName.ToLower().Contains(className.ToLower()) 
            }

            return returnVal.ToArray();
        }


        public static List<IViewModelInfo> GetDBViewInfos(int mainEntityId, bool condensed, int processId)
        {
            processedVMIds.Clear();
            var res = new ConcurrentDictionary<string,IViewModelInfo>();
            using (var ctx = new GenSoftDBContext())
            {
                var list = ctx.EntityRelationship
                    .Include(x => x.EntityTypeAttributes.EntityType.Type)
                    .Include(x => x.EntityTypeAttributes.EntityType).ThenInclude(x => x.EntityTypeViewModelCommand).ThenInclude(x => x.ViewModelCommands.CommandType)
                    .Include(x => x.RelationshipType.ChildOrdinalitys)
                    .Include(x => x.RelationshipType.ParentOrdinalitys)
                    .Include(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.ParentEntity.EntityTypeAttributes.EntityType.Type)
                    .Include(x => x.ParentEntity.EntityTypeAttributes.EntityType).ThenInclude(x => x.EntityTypeViewModelCommand).ThenInclude(x => x.ViewModelCommands.CommandType)
                    .Include(x => x.ParentEntity.EntityTypeAttributes.Attributes)
                    .Where(x => x.ParentEntity.EntityTypeAttributes.EntityType.Id == mainEntityId || x.EntityTypeAttributes.EntityType.Id == mainEntityId)
                    .GroupBy(x => x.ParentEntity.EntityTypeAttributes.EntityType)
                    .ToList();

                foreach (var g in list)
                {
                    var pm = CreateEntityTypeViewModel(g.Key, new List<EntityRelationship>(), true, ctx, processId);
                    var pv = CreateEntityViewModel(ctx, pm, res);
                    if (pv == null) continue;
                    foreach (var rel in g)
                    {
                        var cm = CreateEntityTypeViewModel(rel.EntityTypeAttributes.EntityType, new List<EntityRelationship>() { rel }, rel.RelationshipType.ChildOrdinalityId == 2, ctx, processId);
                        var cv = CreateEntityViewModel(ctx, cm, res);
                        if (cv != null)
                        {
                            if (rel.RelationshipType.ChildOrdinalitys.Name == "One" || condensed)
                            {
                                pv.ViewModelInfos.Add(cv);
                            }
                            else
                            {
                                res.AddOrUpdate(cm.EntityTypeName, cv, (k,v) => cv);
                            }


                        }

                    }
                    res.AddOrUpdate(pm.EntityTypeName, pv, (k, v) => pv);
                }
            }
            return res.Values.ToList();
        }

        private static EntityTypeViewModel CreateEntityTypeViewModel(EntityType entityType, List<EntityRelationship> relationships, bool isList, GenSoftDBContext ctx, int processId)
        {
            return new EntityTypeViewModel()
            {
                Description = entityType.EntitySetName,
                SystemProcessId = processId,
                EntityTypeName = entityType.Type.Name,
                ViewModelTypeName = ctx.ViewModelTypes.First(z => z.DomainEntity == true && z.List == isList).Name,
                EntityTypeViewModelCommands = entityType.EntityTypeViewModelCommand.ToList(),
                EntityViewModelRelationships = relationships.Select(x => new EntityViewModelRelationship()
                {
                    ParentType = x.ParentEntity.EntityTypeAttributes.EntityType.Type.Name,
                    ParentProperty = x.ParentEntity.EntityTypeAttributes.Attributes.Name,
                    ChildType = x.EntityTypeAttributes.EntityType.Type.Name,
                    ChildProperty = x.EntityTypeAttributes.Attributes.Name,

                }).ToList()
            };
        }



        static Dictionary<string, IViewModelInfo> processedVMIds = new Dictionary<string, IViewModelInfo>();

        private static IViewModelInfo CreateEntityViewModel(GenSoftDBContext ctx, EntityTypeViewModel vm, ConcurrentDictionary<string, IViewModelInfo> viewInfos, bool isChild = false)
        {
            if (processedVMIds.ContainsKey(vm.EntityTypeName)) return processedVMIds[vm.EntityTypeName];




            var vp = CreateViewAttributeDisplayProperties(vm);
            vm.Priority = processedVMIds.Count();

            var res = ProcessViewModels.ProcessViewModelFactory[vm.ViewModelTypeName].Invoke(vm, vp);

            processedVMIds.Add(vm.EntityTypeName, res);
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
                    .Select(x => new { x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value) }).ToDictionary(x => x.Key, x => x.Value);

                var viewTypeTheme =
                    ctx.ViewModelPropertyPresentationType
                        .Include(x => x.ViewPropertyValueOptions)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType && x.ViewModelTypes.Name == vm.ViewModelTypeName)
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

        public static List<IComplexEventAction> GetDBComplexActions(int mainEntityId, int processId)
        {

            try
            {
                var res = new ConcurrentBag<IComplexEventAction>();


                using (var ctx = new GenSoftDBContext())
                {

                    var list = ctx.EntityRelationship
                        .Include(x => x.EntityTypeAttributes.EntityType.Type)
                        .Include(x => x.RelationshipType.ChildOrdinalitys)
                        .Include(x => x.RelationshipType.ParentOrdinalitys)
                        .Include(x => x.EntityTypeAttributes.Attributes)
                        .Include(x => x.ParentEntity.EntityTypeAttributes.EntityType.Type)
                        .Include(x => x.ParentEntity.EntityTypeAttributes.Attributes)
                        .Where(x => x.ParentEntity.EntityTypeAttributes.EntityType.Id == mainEntityId || x.EntityTypeAttributes.EntityType.Id == mainEntityId)
                        .GroupBy(x => x.ParentEntity.EntityTypeAttributes.EntityType)
                        .ToList();

                    foreach (var r in list)
                    {
                        var parentType = r.Key.Type.Name;
                        AddDynamicEntityTypes(parentType);


                        res.Add(Processes.ComplexActions.GetComplexAction("IntializeProcessState",new object[]{processId, DynamicEntityType.DynamicEntityTypes[parentType]}));
                        res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList",
                            new object[] { processId, DynamicEntityType.DynamicEntityTypes[parentType] }));

                        foreach (var rel in r.DistinctBy(x => x.Id))
                        {
                            var parentExpression = rel.ParentEntity.EntityTypeAttributes.Attributes.Name;
                            var childType = rel.EntityTypeAttributes.EntityType.Type.Name;

                            if (!DynamicEntityType.DynamicEntityTypes.ContainsKey(childType))
                            {
                                AddDynamicEntityTypes(childType);
                            }
                            


                            var childExpression = rel.EntityTypeAttributes.Attributes.Name;


                            if (rel.RelationshipType.ChildOrdinalitys.Name == "One")
                            {
                                res.Add(Processes.ComplexActions.GetComplexAction("UpdateState",
                                    new object[] { processId, DynamicEntityType.DynamicEntityTypes[childType] }));

                                res.Add(Processes.ComplexActions.GetComplexAction("RequestState",
                                    new object[] { processId, parentType, childType, childExpression }));
                            }
                            else
                            {
                                //res.Add(Processes.ComplexActions.GetComplexAction("IntializeProcessState",
                                //    new object[]
                                //        {processId, DynamicEntityType.DynamicEntityTypes[childType]}));
                                res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateList",
                                    new object[] { processId, DynamicEntityType.DynamicEntityTypes[childType] }));

                                res.Add(Processes.ComplexActions.GetComplexAction("RequestStateList",
                                    new object[]
                                        {processId, parentType, childType, parentExpression, childExpression}));
                            }
                            res.Add(Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",
                                new object[]
                                    {processId, parentType, childType, parentExpression, childExpression}));

                        }

                    }
                }

                //using (var ctx = new GenSoftDBContext())
                //{

                //    Parallel.ForEach(ctx.EntityRelationship
                //        .Include(
                //            "ChildEntity.EntityType.DomainEntityType.ProcessStateDomainEntityTypes.ProcessState.Process")
                //        .Where(x => x.EntityTypeAttributes.EntityType.CompositeRequest != null)
                //        .GroupBy(x => x.EntityTypeAttributes.EntityType).Where(g => g.Count() > 1), (g) =>
                //    {

                //        foreach (var processId in g.Key.DomainEntityType
                //            .ProcessStateDomainEntityTypes.Select(x => x.ProcessState.Process.Id).Distinct())
                //        {
                //            var childType = g.Key.Type.Name;
                //            var parentEntities = g.Select(p =>
                //                new ViewModelEntity()
                //                {
                //                    EntityType =
                //                        DynamicEntityType.DynamicEntityTypes[p.ParentEntity.EntityType.Type.Name],
                //                    Property = p.EntityTypeAttributes.Attributes.Name
                //                }
                //            ).ToList();

                //            res.Add(Processes.ComplexActions.GetComplexAction("RequestCompositeStateList",
                //                new object[]
                //                {
                //                    processId, DynamicEntityType.DynamicEntityTypes[childType],
                //                    new Dictionary<string, dynamic>(), parentEntities
                //                }));
                //        }
                //    });

                //}

                return res.DistinctBy(x => x.Key).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static void AddDynamicEntityTypes(string entityType)
        {

            using (var ctx = new GenSoftDBContext())
            {
                try
                {
                    var viewType = ctx.EntityType
                        .Include(x => x.Type)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attributes)
                        .Include(x => x.EntityTypeAttributes)
                        .Include(x => x.EntityTypeAttributes)
                        .ThenInclude(x => x.CalculatedProperties.CalculatedPropertyParameters)
                        .ThenInclude(x => x.FunctionParameter)
                        .Include(x => x.EntityTypeAttributes)
                        .ThenInclude(x => x.CalculatedProperties.CalculatedPropertyParameters)
                        .ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                        .ThenInclude(x => x.EntityTypeAttributes.Attributes)
                        .Include(x => x.EntityTypeAttributes)
                        .ThenInclude(x => x.CalculatedProperties.FunctionParameterConstant)
                        .Include(x => x.EntityTypeAttributes)
                        .ThenInclude(x => x.CalculatedProperties.FunctionSets.FunctionSetFunctions)
                        .ThenInclude(x => x.Functions)
                        .Include(x => x.EntityTypeAttributes)
                        .ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.EntityTypeAttributeCache)
                        .FirstOrDefault(x => x.Type.Name == entityType);
                    if (viewType == null) return;

                    var viewset = ctx.EntityTypeAttributes
                        .Where(x => x.EntityTypeId == viewType.Id)
                        .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                        .Select(x => x.AttributeId).ToList();

                    if (!viewset.Any()) return;
                    var tes =

                        ctx.Entity
                            .FirstOrDefault(x => x.Id == 0)
                            ?.EntityAttribute
                            .Where(z => viewset.Contains(z.AttributeId))
                            .OrderBy(d => viewset.IndexOf(d.AttributeId))
                            .Select(z => new EntityKeyValuePair(z.Attributes.Name, z.Value,
                                (ViewAttributeDisplayProperties)CreateEntityAttributeViewProperties(z.Id),
                                z.Attributes.EntityId != null, z.Attributes.EntityName != null) as IEntityKeyValuePair)
                            .ToList()
                        ??
                        ctx.EntityTypeAttributes
                            .Where(x => x.EntityTypeId == viewType.Id)
                            .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                            .Select(z =>
                                new EntityKeyValuePair(z.Attributes.Name,
                                    null,
                                    (ViewAttributeDisplayProperties)CreateEntityAttributeViewProperties(z.Id),
                                    z.Attributes.EntityId != null,
                                    z.Attributes.EntityName != null) as IEntityKeyValuePair).ToList();

                    var calPropDef = CreateCalculatedProperties(viewType);

                    var cachedProperties = CreateCachedProperties(viewType);
                    var cachedEntityProperties = CreateCachedEntityProperties(viewType);


                    var dynamicEntityType = new DynamicEntityType(viewType.Type.Name,
                        viewType.EntitySetName, tes, calPropDef, cachedProperties, cachedEntityProperties);



                    DynamicEntityType.DynamicEntityTypes.AddOrSet(entityType, dynamicEntityType);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static ObservableDictionary<string, List<dynamic>> CreateCachedProperties(EntityType viewType)
        {
            var res = new ObservableDictionary<string, List<dynamic>>();


            var lst = viewType.EntityTypeAttributes
                .Where(x => x.EntityTypeAttributeCache != null).DistinctBy(x => x.Id).ToList();


            using (var ctx = new GenSoftDBContext())
            {
                foreach (var cp in lst)
                {
                    var elst = ctx.EntityAttribute
                        .Where(x => x.AttributeId == cp.AttributeId)
                        .Select(x => new { Key = x.Attributes.Name, Value = x.Value })
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

        private static ObservableDictionary<string, string> CreateCachedEntityProperties(EntityType viewType)
        {
            var res = new ObservableDictionary<string, string>();





            //using (var ctx = new GenSoftDBContext())
            //{

            //    var lst = ctx.EntityTypeAttributes
            //        .Include(x => x.Attributes)
            //        .Include(x => x.ParentEntity).ThenInclude(x => x.EntityTypeAttributes.EntityType.Type)
            //        .Where(x => x.EntityTypeId == viewType.Id && !x.ParentEntity.Any())
            //        .Select(x => new { Attribute = x.Attributes.Name, ParentEntities = x.ParentEntity.Select(z => z.EntityTypeAttributes.EntityType).ToList() }).ToList();

            //    foreach (var p in lst)
            //    {
            //        var cache =
            //        ctx.EntityTypeViewModel
            //        .Include(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type)
            //        .Where(x => x.ViewModelTypes.Name == "CachedViewModel" &&
            //                                           p.ParentEntities.Select(z => z.EntityTypeId).Contains(x.ProcessStateDomainEntityTypes
            //                                                   .DomainEntityTypeId))
            //        .Select(x => x.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name).FirstOrDefault();
            //        if (cache != null) res.Add(p.Attribute, cache);
            //    }

            //}


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
                    ctx.EntityTypePresentationProperty
                        .Include(x => x.ViewPropertyValueOptions)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType && x.EntityTypeAttributeId == entityTypeAttributeId)
                        .Select(x => new
                        {
                            PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType
                                .Name,
                            ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                            x.ViewPropertyValueOptions.Value
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


        private static Dictionary<string, List<dynamic>> CreateCalculatedProperties(EntityType viewType)
        {

            var calprops = new Dictionary<string, List<dynamic>>();


            var lst = viewType.EntityTypeAttributes
                .Where(x => x.CalculatedProperties != null)
                .Select(x => x.CalculatedProperties).DistinctBy(x => x.Id).ToList();
            foreach (var cp in lst)
            {
                var flst = cp
                    .FunctionSets
                    .FunctionSetFunctions
                    .DistinctBy(x => x.Id)
                    .Select(f =>
                        DynamicEntityType.Functions.ContainsKey(f.Functions.Name)
                            ? DynamicEntityType.Functions[f.Functions.Name]
                            : CreateFunction(
                                CreateCPFunctionBody(
                                    f.Functions.Body,
                                    f.Functions.FunctionParameter.ToList(),
                                    cp.Id),
                                f.Functions.FunctionParameter.ToList(),
                                f.Functions.ReturnDataTypeId)).ToList();

                calprops.Add(cp.EntityTypeAttributes.Attributes.Name, flst);
            }


            return calprops;
        }
    }


}