using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using SystemInterfaces;
using Actor.Interfaces;
using Akka.Actor;
using Akka.Util.Internal;
using Common;
using DynamicExpresso;
using EventAggregator;
using EventMessages.Commands;
using GenSoft.DBContexts;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Process.WorkFlow;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using Utilities;
using ViewModel.Interfaces;
using ViewModel.WorkFlow;
using Action = GenSoft.Entities.Action;
using ActionTrigger = Actor.Interfaces.ActionTrigger;
using ComplexEventAction = RevolutionEntities.Process.ComplexEventAction;
using IComplexEventAction = Actor.Interfaces.IComplexEventAction;
using IProcessAction = Actor.Interfaces.IProcessAction;
using IStateCommandInfo = SystemInterfaces.IStateCommandInfo;
using ISystemProcess = SystemInterfaces.ISystemProcess;
using IUser = SystemInterfaces.IUser;
using ProcessAction = GenSoft.Entities.ProcessAction;
using SystemProcess = RevolutionEntities.Process.SystemProcess;
using DomainUtilities;
using Application = GenSoft.Entities.Application;

namespace DataServices.Actors
{
    public class DomainProcessSupervisor : BaseSupervisor<DomainProcessSupervisor>
    {
        private IUntypedActorContext actorCtx = null;
       

        
        public static ConcurrentDictionary<string, Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>>> ProcessActions = new ConcurrentDictionary<string, Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>>>();
        public static ConcurrentDictionary<string, StateCommand> StateCommands { get; } = new ConcurrentDictionary<string, StateCommand>();
        public static ConcurrentDictionary<string, StateEvent> StateEvents { get; } = new ConcurrentDictionary<string, StateEvent>();

        //TODO: Track Actor Shutdown instead of just broadcast


        static int maxProcessId = 0;

        
         private static Application CurrentApplication { get; set; }  
        

        private void OnCurrentApplicationChanged(ICurrentApplicationChanged currentEntityChanged)
        {
            if (currentEntityChanged.Application == null) return;
            if (CurrentApplication?.Id == currentEntityChanged.Application.Id) return;
            using (var ctx = new GenSoftDBContext())
            {
                CurrentApplication = ctx.Application.Include(x => x.DatabaseInfo)
                    .First(x => x.Id == currentEntityChanged.Application.Id);
            }
            LoadProcesses();
        }

        public DomainProcessSupervisor(bool autoRun, ISystemProcess process) : base(process)
        {
            EventMessageBus.Current.GetEvent<ICurrentApplicationChanged>(Source).Subscribe(OnCurrentApplicationChanged);

            BuildExpressions();
            
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(Source).Where(x => autoRun && x.ProcessToBeStartedId == RevolutionData.ProcessActions.NullProcess).Subscribe(x => StartParentProcess(x.Process.Id, x.User));
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(Source).Where(x => !autoRun && x.ProcessToBeStartedId != RevolutionData.ProcessActions.NullProcess).Subscribe(x => StartProcess(x.ProcessToBeStartedId, x.User));

            EventMessageBus.Current.GetEvent<IMainEntityChanged>(Source).Subscribe(OnMainEntityChanged);
           

            actorCtx = Context;
        }

        private void LoadProcesses()
        {
            List<EntityType> mainEntities;
           
            using (var ctx = new GenSoftDBContext())
            {
                if(maxProcessId == 0) maxProcessId = ctx.SystemProcess.Max(x => x.Id);

                _domainProcessLst = ctx.DomainProcess
                    .Include(x => x.SystemProcess)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.MainEntity.EntityType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ActionTrigger)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.ExpectedEvents.EventType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.StateEventInfo.StateInfo)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.ExpectedEvents.ExpectedEventPredicateParameters).ThenInclude(x => x.EventPredicates)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.ExpectedEvents.ExpectedEventPredicateParameters)
                    .ThenInclude(x => x.ExpectedEventConstants)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.ExpectedEvents.EventType.EventPredicates)
                    .ThenInclude(x => x.Predicates.PredicateParameters).ThenInclude(x => x.ExpectedEventPredicateParameters)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents)
                    .ThenInclude(x => x.ExpectedEvents.EventType.EventPredicates)
                    .ThenInclude(x => x.Predicates.PredicateParameters).ThenInclude(x => x.Parameters)
                    .ThenInclude(x => x.DataType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions).ThenInclude(x => x.EventType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ComplexEventAction).ThenInclude(x => x.ProcessStepComplexActions)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ProcessAction.Action.ActionParameters)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ProcessAction.Action.ActionParameters).ThenInclude(x => x.Parameters)
                    .ThenInclude(x => x.DataType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ProcessAction.Action.ActionReferenceTypes)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ProcessAction.Action.ActionReferenceTypes)
                    .ThenInclude(x => x.ReferenceTypes.DataType.Type)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions)
                    .ThenInclude(x => x.ProcessAction.Action.ActionReferenceTypes)
                    .ThenInclude(x => x.ReferenceTypes.ReferenceTypeName)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.ProcessStepComplexActions)
                    .ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions).ThenInclude(x =>
                        x.ProcessAction.ProcessActionStateCommandInfo.StateCommandInfo.StateInfo)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .Where(x => x.ProcessStep.Any(z => z.MainEntity != null))
                    .Where(x => x.ApplicationId == CurrentApplication.Id)
                    .ToList();
                if (!_domainProcessLst.Any())
                {
                    SystemProcess systemProcess;
                    maxProcessId += 1;

                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, Process, "AutoSystemProcess",
                            "AutoSystemProcess", "Auto", Process.User.UserId), Process.User, Process.MachineInfo);
                   
                    var parentEntityTypes = ctx.EntityRelationship.Where(x => x.EntityTypeAttributes.EntityType.ApplicationId == CurrentApplication.Id).Select(x => x.ParentEntity.EntityTypeAttributes.EntityType)
                        .Distinct().ToList();
                    var childEntityTypes = ctx.EntityRelationship.Where(x => x.EntityTypeAttributes.EntityType.ApplicationId == CurrentApplication.Id).Select(x => x.EntityTypeAttributes.EntityType).Distinct()
                        .ToList();
                    mainEntities = parentEntityTypes.Where(z => !childEntityTypes.Contains(z)).ToList();
                    Task.Run(() =>
                    {
                        IntializeProcess(systemProcess);
                    });
                    foreach (var mainEntity in mainEntities)
                    {
                        Task.Run(() =>
                        {
                            PublishComplexEvents(systemProcess, GetDBComplexActions(mainEntity.Id, systemProcess));
                        });
                        Task.Run(() =>
                        {
                            PublishViewModels(systemProcess, GetDBViewInfos(mainEntity.Id, true, systemProcess));
                        });
                    }

                    
                }
                else
                {
                    LoadDomainProcess(_domainProcessLst.FirstOrDefault(), Process.User);
                }
            }
        }

        private List<DomainProcess> _domainProcessLst= new List<DomainProcess>();

        private void StartProcess(int objProcessToBeStartedId, IUser user)
        {
            var dp = _domainProcessLst.FirstOrDefault(x => x.Id == objProcessToBeStartedId);
            if (dp != null) LoadDomainProcess(dp,user);
        }

        private void StartParentProcess(int processId, IUser user)
        {
            var dp = _domainProcessLst.FirstOrDefault(x => x.SystemProcess.ParentSystemProcess.ParentProcessId == processId);
            if(dp != null) LoadDomainProcess(dp, user);
        }

        private void LoadDomainProcess(DomainProcess domainProcess, IUser user)
        {
            
            var systemProcess = new SystemProcess(
                new SystemProcessInfo(domainProcess.Id, new RevolutionEntities.Process.SystemProcess(new RevolutionEntities.Process.Process(domainProcess.SystemProcess.ParentSystemProcess.Id, Processes.NullSystemProcess, domainProcess.SystemProcess.ParentSystemProcess.SystemProcess.Name, domainProcess.SystemProcess.ParentSystemProcess.SystemProcess.Description, domainProcess.SystemProcess.ParentSystemProcess.SystemProcess.Symbol, new RevolutionEntities.Process.Agent(domainProcess.SystemProcess.ParentSystemProcess.SystemProcess.Agent.UserName)), Processes.ThisMachineInfo),
                    domainProcess.SystemProcess.Name,
                    domainProcess.SystemProcess.Description, domainProcess.SystemProcess.Symbol,
                    user.UserId), user, Process.MachineInfo);

            //// Do not intitalize domain specified processes IntializeProcess(domainProcess, systemProcess);
            //List<IComplexEventAction> processComplexEvents = new List<IComplexEventAction>
            //{
            //    Processes.ComplexActions.GetComplexAction("ProcessStarted", new object[] {systemProcess}),
            //    Processes.ComplexActions.GetComplexAction("CleanUpProcess", new object[] {systemProcess})
            //};
            //PublishComplexEvents(systemProcess, processComplexEvents);

            Task.Run(() => { IntializeProcess(systemProcess); });

            Parallel.ForEach(domainProcess.ProcessStep,
                new ParallelOptions() {MaxDegreeOfParallelism = Processes.ThisMachineInfo.Processors},
                (processStep) =>
                {
                    //            foreach (var processStep in domainProcess.ProcessStep)
                    //{
                    if (processStep.MainEntity == null) return;

                    Parallel.ForEach(processStep.ProcessStepComplexActions,
                        new ParallelOptions() {MaxDegreeOfParallelism = Processes.ThisMachineInfo.Processors},
                        (pcp) =>
                        {
                            //foreach (var pcp in processStep.ProcessStepComplexActions)
                            //{
                            var cpEvents = new List<IProcessExpectedEvent>();
                            foreach (var ce in pcp.ComplexEventAction.ComplexEventActionExpectedEvents)
                            {
                                cpEvents.Add(CreateProcessExpectedEvent(pcp.ProcessStep.DomainProcessId, ce,
                                    processStep.MainEntity.EntityType));
                            }
                            foreach (var cp in pcp.ComplexEventAction.ComplexEventActionProcessActions)
                            {
                                var complexEventAction =
                                    CreateComplexEventAction(systemProcess, cp, cpEvents);
                                PublishComplexEvents(systemProcess,
                                    new List<IComplexEventAction>() {complexEventAction});
                            }
                            //}
                        });
                    Task.Run(() =>
                    {
                        PublishViewModels(systemProcess,
                            GetDBViewInfos(processStep.MainEntity.EntityType.Id, false, systemProcess));
                    });
                    Task.Run(() =>
                    {
                        PublishComplexEvents(systemProcess,
                            GetDBComplexActions(processStep.MainEntity.EntityType.Id, systemProcess));
                    });
                    //}
                });

        }

        private void IntializeProcess(SystemProcess systemProcess)
        {
            var processComplexEvents = new List<IComplexEventAction>
            {
                Processes.ComplexActions.GetComplexAction("StartNextProcess", new object[] {systemProcess}),
                Processes.ComplexActions.GetComplexAction("ProcessStarted", new object[] {systemProcess}),
                Processes.ComplexActions.GetComplexAction("CleanUpProcess",new object[] {systemProcess})
               // Processes.ComplexActions.GetComplexAction("CleanUpParentProcess",new object[] {systemProcess.ParentProcess, systemProcess})
            };
            PublishComplexEvents(systemProcess, processComplexEvents);
        }

        private IProcessExpectedEvent CreateProcessExpectedEvent(int processId, ComplexEventActionExpectedEvents ce, EntityType entityType)
        {
            var eventType = TypeNameExtensions.GetTypeByName(ce.ExpectedEvents.EventType.Type.Name).FirstOrDefault();
            var res = typeof(DomainProcessSupervisor).GetMethod("ProcessExpectedEvent").MakeGenericMethod(eventType)
                .Invoke(null, new object[] {processId, ce, entityType});
            return res as IProcessExpectedEvent;
        }

        public static IProcessExpectedEvent ProcessExpectedEvent<TEventType>(ISystemProcess process, ComplexEventActionExpectedEvents ce,EntityType entityType) where TEventType: IProcessSystemMessage
        {
            Func <TEventType, bool> eventPredicate = CreateExpectedEventPredicate<TEventType>(ce.ExpectedEvents);
            
            return new ProcessExpectedEvent<TEventType>(process: process,
                eventPredicate: eventPredicate,
                processInfo: new RevolutionEntities.Process.StateEventInfo(process,
                    StateEvents[ce.StateEventInfo.StateInfo.Name]),
                expectedSourceType: new RevolutionEntities.Process.SourceType(typeof(IComplexEventService)),
                key: $"{ce.ExpectedEvents.EventType.Type.Name}-{entityType.Type.Name}");
        }

        private static Func<TEventType, bool> CreateExpectedEventPredicate<TEventType>(ExpectedEvents expectedEvents) where TEventType : IProcessSystemMessage
            {
            //e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey("Password")
            foreach (var ep in expectedEvents.EventType.EventPredicates)
            {
                var expectedEventPredicateParameters = expectedEvents.ExpectedEventPredicateParameters.Where(x => x.EventPredicateId == ep.Id).ToList();
                var body = CreateExpectedEventPredicateBody(ep.Predicates, expectedEventPredicateParameters);
                var predicateParameters = expectedEventPredicateParameters.Select(x => x.PredicateParameters).ToList();
                var res = DynamicEntityTypeExtensions.CreatePredicate(body, predicateParameters);
                return res;
            }
            return null;
            
        }



        private static string CreateExpectedEventPredicateBody(Predicates predicates, List<ExpectedEventPredicateParameters> predicateParameters)
        {
            var paramlst = new Dictionary<string, string>();
            foreach (var p in predicateParameters)
            {
                if (p.ExpectedEventConstants != null)
                {
                    var c = p.ExpectedEventConstants.Value;
                    paramlst.AddOrSet(p.PredicateParameters.Parameters.Name, $"\"{c}\"");
                }

                //var pEntityParameters =
                    //    cp.CalculatedPropertyParameters.Where(x => x.FunctionParameterId == p.Id)
                    //        .DistinctBy(x => x.Id).ToList();
                    //for (int j = 0; j < pEntityParameters.Count(); j++)
                    //{
                    //    var param = pEntityParameters[j];
                    //    var cparameter = param.CalculatedPropertyParameterEntityTypes.DistinctBy(x => x.Id)
                    //        .FirstOrDefault(x => x.CalculatedPropertyParameterId == param.Id)
                    //        ?.EntityTypeAttributes.Attributes.Name;

                    //    if (cparameter != null) paramlst.AddOrSet($"param{j}", $"\"{cparameter}\"");
                    //}
                
            }
            var newBody = paramlst.Aggregate(predicates.Body, (current, p) => current.Replace(p.Key, p.Value));

            return newBody;
        }

        private void OnMainEntityChanged(IMainEntityChanged mainEntityChanged)
        {
         
            using (var ctx = new GenSoftDBContext())
            {
                 var entityType = ctx.EntityType.First(x => x.Type.Name == mainEntityChanged.EntityType.Name);
                 var domainProcess = ctx.DomainProcess
                    .Include(x => x.SystemProcess)
                    .Include(x => x.ProcessStep).ThenInclude(x => x.MainEntity.EntityType)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .FirstOrDefault(x => x.ProcessStep.Any(z => z.MainEntity.EntityType == entityType));
                SystemProcess systemProcess;
                
                maxProcessId += 1;
                if (domainProcess == null)
                {
                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, mainEntityChanged.Process, $"AutoProcess-{entityType.EntitySetName}",
                            $"AutoProcess-{entityType.EntitySetName}", "Auto", mainEntityChanged.User.UserId), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                }
                else
                {
                    systemProcess = new SystemProcess(
                    new SystemProcessInfo(maxProcessId, mainEntityChanged.Process, domainProcess.SystemProcess.Name,
                        domainProcess.SystemProcess.Description, domainProcess.SystemProcess.Symbol, mainEntityChanged.User.UserId), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                }
                Task.Run(() => { IntializeProcess(systemProcess);});
                Task.Run(() => { PublishViewModels(systemProcess,GetDBViewInfos(entityType.Id, false, systemProcess));});
                Task.Run(() => { PublishComplexEvents(systemProcess,GetDBComplexActions(entityType.Id, systemProcess));});
            }
        }

      

        
        
        private void PublishComplexEvents(SystemProcess systemProcess, List<IComplexEventAction> processComplexEvents)
        {
            var inMsg = new LoadProcessComplexEvents(processComplexEvents,
                new RevolutionEntities.Process.StateCommandInfo(systemProcess, RevolutionData.Context.Process.Commands.StartProcess),
                systemProcess, Source);

            EventMessageBus.Current.Publish(inMsg, Source);
        }

        private void PublishViewModels(SystemProcess systemProcess, List<IViewModelInfo> viewModelInfos)
        {
            var inMsg = new LoadDomainProcessViewModels(viewModelInfos,
                new RevolutionEntities.Process.StateCommandInfo(systemProcess, RevolutionData.Context.Process.Commands.StartProcess),
                systemProcess, Source);

            EventMessageBus.Current.Publish(inMsg, Source);
        }


        private void BuildExpressions()
        {
            BuildFunctions();
           // BuildActions();
            BuildStateCommandInfo();
            BuildStateEventInfo();
            //BuildProcessActions();
      
        }


        private ComplexEventAction CreateComplexEventAction(ISystemProcess process, ComplexEventActionProcessActions complexEventAction, IList<IProcessExpectedEvent> cpEvents)
        {
            return new ComplexEventAction(
                key: $"CustomComplexEvent:{complexEventAction.ComplexEventAction.Name}-{complexEventAction.ProcessAction.Name}",
                process: process,
                actionTrigger: complexEventAction.ComplexEventAction.ActionTrigger.Name == "Any"?ActionTrigger.Any:ActionTrigger.All,
                events: cpEvents,
                expectedMessageType: typeof(SystemInterfaces.IEntity).Assembly.GetType($"SystemInterfaces.{complexEventAction.EventType.Type.Name}"),
                action: CreateProcessAction(process,complexEventAction.ProcessAction),
                processInfo: new RevolutionEntities.Process.StateCommandInfo(process, StateCommands[complexEventAction.ProcessAction.ProcessActionStateCommandInfo.StateCommandInfo.StateInfo.Name]));
        }

        private IProcessAction CreateProcessAction(ISystemProcess process, ProcessAction processAction)
        {
            return new RevolutionEntities.Process.ProcessAction(CreateComplexEventParametersAction(processAction.Action),
                processAction.ProcessActionStateCommandInfo != null
                ? (cp => new RevolutionEntities.Process.StateCommandInfo(cp.Actor.Process, StateCommands[processAction.ProcessActionStateCommandInfo.StateCommandInfo.StateInfo.Name]))
                : CreateComplexEventParameterStateCommand(process, processAction.Action),
                new RevolutionEntities.Process.SourceType(typeof(IComplexEventService)));
        }

        private Func<IDynamicComplexEventParameters, IStateCommandInfo> CreateComplexEventParameterStateCommand(ISystemProcess processId, Action stateCommand)
        {
            throw new NotImplementedException();
        }

        private Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>> CreateComplexEventParametersAction(
            Action action)
        {
            try
            {
                var actions = new List<Func<IDynamicComplexEventParameters, IProcessSystemMessage>>();

                
                    var interpreter = new Interpreter();
                    foreach (var r in action.ActionReferenceTypes)
                    {
                        if (r.ReferenceTypes.ReferenceTypeName != null)
                        {
                            interpreter.Reference(new ReferenceType(r.ReferenceTypes.ReferenceTypeName.Name,
                                TypeNameExtensions.GetTypeByName(r.ReferenceTypes.DataType.Type.Name)[0]));
                        }
                        else
                        {
                            interpreter.Reference(
                                TypeNameExtensions.GetTypeByName(r.ReferenceTypes.DataType.Type.Name)[0]);
                        }
                    }
                interpreter.Reference(typeof(SystemProcess));
                
                var body = action.Body;
                var res = interpreter.ParseAsDelegate<Func<IDynamicComplexEventParameters, IProcessSystemMessage>>(body, action.ParameterName);
               var restuple = new Tuple<Action, Func<IDynamicComplexEventParameters, IProcessSystemMessage>>(action, res);
             
                return async cp => await Task.Run(() => restuple.Item2(cp));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
        }

        private void BuildProcessActions()
        {
            throw new NotImplementedException();
        }

        private void BuildStateEventInfo()
        {
            try
            {
                using (var ctx = new GenSoftDBContext())
                {
                    var lst = ctx.StateEventInfo
                        .Include(x => x.StateInfo)
                        .Include(x => x.StateCommandInfo.StateInfo)
                        .ToList();

                    foreach (var f in lst.Where(x => !StateEvents.ContainsKey(x.StateInfo.Name)).ToList())
                    {
                        CreateStateEvent(f, StateCommands[f.StateCommandInfo.StateInfo.Name]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private StateEvent CreateStateEvent(GenSoft.Entities.StateEventInfo f, StateCommand command)
        {
            if (StateEvents.ContainsKey(f.StateInfo.Name))return StateEvents[f.StateInfo.Name];

            var stateEvent = new StateEvent(f.StateInfo.Name, f.StateInfo.Status, f.StateInfo.StateInfoNotes?.Notes, command);
            StateEvents.AddOrSet(f.StateInfo.Name, stateEvent);
            return stateEvent;
        }

        private void BuildStateCommandInfo()
        {
            try
            {
                using (var ctx = new GenSoftDBContext())
                {
                    var lst = ctx.StateCommandInfo
                        .Include(x => x.StateInfo)
                        .Include(x => x.ExpectedStateEventInfo.StateEventInfo.StateInfo)
                        .ToList();

                    foreach (var f in lst)
                    {

                        var res = new StateCommand(f.StateInfo.Name, f.StateInfo.Status);
                        if (f.ExpectedStateEventInfo?.StateEventInfo != null)
                        {
                            var se = CreateStateEvent(f.ExpectedStateEventInfo.StateEventInfo, res);
                            res.ExpectedEvent = se;
                        }
                        StateCommands.AddOrSet(f.StateInfo.Name, res);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static void BuildActions()
        {
            using (var ctx = new GenSoftDBContext())
            {
                var lst = ctx.Action
                    .Include(x => x.ActionParameters).ThenInclude(x => x.ActionPropertyParameter)
                    .ToList();

                foreach (var f in lst)
                {
                    var actionParameters = f.ActionParameters.ToList();

                    var res = CreateAction(f.Body, actionParameters);
                    DynamicEntityTypeExtensions.Functions.Add(f.Name, res);
                }
            }
        }

        private static dynamic CreateAction(string body, List<ActionParameters> actionParameters)
        {
            throw new NotImplementedException();
        }

        private static void BuildFunctions()
        {
            using (var ctx = new GenSoftDBContext())
            {
                var lst = ctx.Functions
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.FunctionParameterConstant)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.FunctionParameter)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                    .ThenInclude(x => x.EntityTypeAttributes.Attributes)
                    .Include(x => x.FunctionParameter).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperties).ThenInclude(x => x.FunctionParameterConstant)
                    .Where(x => x.FunctionParameter.All(z =>
                        !z.CalculatedPropertyParameters.Any() && !z.FunctionParameterConstant.Any()))
                    .ToList();

                foreach (var f in lst)
                {
                    var FunctionParameter = f.FunctionParameter
                        .DistinctBy(x => x.Id).ToList();

                    var res = DynamicEntityTypeExtensions.CreateFunction(f.Body, FunctionParameter, f.ReturnDataTypeId);
                    DynamicEntityTypeExtensions.Functions.Add(f.Name, res);
                }
            }
        }
     






        public static List<IViewModelInfo> GetDBViewInfos(int mainEntityId, bool condensed, ISystemProcess process)
        {
           
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
                    var pm = CreateEntityTypeViewModel(g.Key, new List<EntityRelationship>(), true, ctx, process, EntityRelationshipOrdinality.One);
                    var pv = CreateEntityViewModel(pm);
                    if (pv == null) continue;
                    res.AddOrUpdate(pm.EntityTypeName, pv);
                    var ppm = CreateEntityTypeViewModel(g.Key, new List<EntityRelationship>(), false, ctx, process, EntityRelationshipOrdinality.One);
                    var ppv = CreateEntityViewModel(ppm);
                    pv.ViewModelInfos.Add(ppv);
                    foreach (var rel in g)
                    {
                        var cm = CreateEntityTypeViewModel(rel.EntityTypeAttributes.EntityType, new List<EntityRelationship>() { rel }, rel.RelationshipType.ChildOrdinalityId == 2, ctx, process, rel.RelationshipType.ChildOrdinalitys.Name == "One" ? EntityRelationshipOrdinality.One : EntityRelationshipOrdinality.Many);
                        var cv = CreateEntityViewModel(cm);
                        if (cv != null)
                        {
                            if (rel.RelationshipType.ChildOrdinalitys.Name == "One" || condensed)
                            {
                                cv.Visibility = Visibility.Visible;
                                pv.ViewModelInfos.Add(cv);
                            }
                            else
                            {
                                res.AddOrUpdate(cm.EntityTypeName, cv);
                                cv.Visibility = Visibility.Collapsed;
                                pv.ViewModelInfos.Add(cv);
                            }


                        }

                    }
                   
                }
                if (list.Any()) return res.Values.ToList();
                {
                    var mainEntity = ctx.EntityType
                        .Include(x => x.Type)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attributes)
                        .Include(x => x.EntityTypeViewModelCommand).ThenInclude(x => x.ViewModelCommands.CommandType)
                        .First(x => x.Id == mainEntityId);
                    var ppm = CreateEntityTypeViewModel(mainEntity, new List<EntityRelationship>(), false, ctx, process, EntityRelationshipOrdinality.One);
                    var ppv = CreateEntityViewModel(ppm);
                    res.AddOrUpdate(ppm.EntityTypeName, ppv);
                }
            }
            return res.Values.ToList();
        }

        private static EntityTypeViewModel CreateEntityTypeViewModel(EntityType entityType, List<EntityRelationship> relationships, bool isList, GenSoftDBContext ctx, ISystemProcess process, EntityRelationshipOrdinality ordinality)
        {
            return new EntityTypeViewModel()
            {
                Description = entityType.EntitySetName,
                SystemProcess = process,
                EntityTypeName = entityType.Type.Name,
                RelationshipOrdinality = ordinality,
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

        

        private static IViewModelInfo CreateEntityViewModel(EntityTypeViewModel vm)
        {
           
            var vp = CreateViewAttributeDisplayProperties(vm);
            var res = ProcessViewModels.ProcessViewModelFactory[vm.ViewModelTypeName].Invoke(vm, vp);
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

        public List<IComplexEventAction> GetDBComplexActions(int mainEntityId, ISystemProcess process)
        {
            
            try
            {
                var res = new ConcurrentDictionary<string,IComplexEventAction>();


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
                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType);


                        res.AddOrUpdate($"{parentType}-IntializeProcessStateList",Processes.ComplexActions.GetComplexAction("IntializeProcessStateList",new object[]{ process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType)}));
                        res.AddOrUpdate($"{parentType}-UpdateStateList",Processes.ComplexActions.GetComplexAction("UpdateStateList",new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType) }));
                        res.AddOrUpdate($"{parentType}-UpdateState", Processes.ComplexActions.GetComplexAction("UpdateState",new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType) }));
                        res.AddOrUpdate($"{parentType}|{parentType}-RequestState", Processes.ComplexActions.GetComplexAction("RequestState",new object[] { process, parentType, parentType, "Id" }));

                        var entityRelationships = r.DistinctBy(x => x.Id);
                        foreach (var rel in entityRelationships)
                        {
                            var parentExpression = rel.ParentEntity.EntityTypeAttributes.Attributes.Name;
                            var childType = rel.EntityTypeAttributes.EntityType.Type.Name;

                            if (DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType) != null) 
                            {
                                DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType);
                                if (rel.RelationshipType.ChildOrdinalitys.Name != "One")
                                {
                                    DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType).ChildEntities
                                        .Add(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType));
                                    DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType).ParentEntities
                                        .Add(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType));
                                }
                            }
                            


                            var childExpression = rel.EntityTypeAttributes.Attributes.Name;


                            if (rel.RelationshipType.ChildOrdinalitys.Name == "One")
                            {
                                res.AddOrUpdate($"{childType}-UpdateState", Processes.ComplexActions.GetComplexAction("UpdateState",
                                    new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType) }));

                                res.AddOrUpdate($"{parentType}|{childType}-RequestState", Processes.ComplexActions.GetComplexAction("RequestState",
                                    new object[] { process, parentType, childType, childExpression }));
                            }
                            else
                            {
                                
                                //res.Add(Processes.ComplexActions.GetComplexAction("IntializeProcessState",
                                //    new object[]
                                //        {processId, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType]}));
                                res.AddOrUpdate($"{childType}-UpdateStateList", Processes.ComplexActions.GetComplexAction("UpdateStateList",
                                    new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType) }));

                                res.AddOrUpdate($"{parentType}|{childType}-RequestStateList", Processes.ComplexActions.GetComplexAction("RequestStateList",
                                    new object[]
                                        {process, parentType, childType, parentExpression, childExpression}));
                            }
                            res.AddOrUpdate($"{parentType}|{childType}-UpdateStateWhenDataChanges", Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",
                                new object[]
                                    {process, parentType, childType, parentExpression, childExpression}));

                        }

                    }
                    if (list.Any()) return res.Values.ToList();
                   
                        var mainEntity = ctx.EntityType
                            .Include(x => x.Type)
                            .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attributes)
                            .First(x => x.Id == mainEntityId);
                        var mainEntityType = mainEntity.Type.Name;
                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType);

                        res.AddOrUpdate($"{mainEntityType}-IntializeProcessState", Processes.ComplexActions.GetComplexAction("IntializeProcessState", new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType) }));
                        res.AddOrUpdate($"{mainEntityType}-UpdateState", Processes.ComplexActions.GetComplexAction("UpdateState", new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType) }));
                        res.AddOrUpdate($"{mainEntityType}|{mainEntityType}-RequestState", Processes.ComplexActions.GetComplexAction("RequestState", new object[] { process, mainEntityType, mainEntityType, "Id" }));
                    
                }
                

                return res.Values.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        


    }


}