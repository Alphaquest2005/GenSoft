using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SystemInterfaces;
using Actor.Interfaces;

using Common;
using Common.DataEntites;
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
using EventMessages.Events;
using Application = GenSoft.Entities.Application;
using ReferenceType = DynamicExpresso.ReferenceType;

namespace DataServices.Actors
{
    public class DomainProcessSupervisor : BaseSupervisor<DomainProcessSupervisor>
    {
        
       

        
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
                CurrentApplication = ctx.Applications.Include(x => x.DatabaseInfo)
                    .First(x => x.Id == Convert.ToInt32(currentEntityChanged.Application.Id));
            }
            _processComplexEvents.Clear();
            DynamicEntityTypeExtensions.ResetDynamicTypes();
            LoadProcesses();
        }

        public DomainProcessSupervisor(bool autoRun, ISystemProcess process) : base(process)
        {
            var currentApplicationChangedProcessStateInfo = new RevolutionEntities.Process.StateEventInfo(process,RevolutionData.Context.Process.Events.CurrentApplicationChanged, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<ICurrentApplicationChanged>(currentApplicationChangedProcessStateInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == currentApplicationChangedProcessStateInfo.EventKey)
                .Subscribe(OnCurrentApplicationChanged);

            Task.Run(() => { BuildExpressions();}).ConfigureAwait(false);

            var startSystemProcessCommandInfo = new RevolutionEntities.Process.StateCommandInfo(process, RevolutionData.Context.Process.Commands.StartProcess, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(startSystemProcessCommandInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == startSystemProcessCommandInfo.EventKey)
                .Where(x => autoRun && x.ProcessToBeStartedId == RevolutionData.ProcessActions.NullProcess)
                .Subscribe(x => StartParentProcess(x.Process.Id, x.User));


            var startParentProcessCommandInfo = new RevolutionEntities.Process.StateCommandInfo(process, RevolutionData.Context.Process.Commands.StartProcess, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<IStartSystemProcess>(startParentProcessCommandInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == startParentProcessCommandInfo.EventKey)
                .Where(x => !autoRun && x.ProcessToBeStartedId != RevolutionData.ProcessActions.NullProcess)
                .Subscribe(x => StartProcess(x.ProcessToBeStartedId, x.User));


            var mainEntityChangedCommandInfo = new RevolutionEntities.Process.StateCommandInfo(process, RevolutionData.Context.Process.Commands.ChangeMainEntity, Guid.NewGuid());
            EventMessageBus.Current.GetEvent<IMainEntityChanged>(mainEntityChangedCommandInfo, Source)
                .Where(x => x.ProcessInfo.EventKey == Guid.Empty || x.ProcessInfo.EventKey == mainEntityChangedCommandInfo.EventKey)
                .Subscribe(OnMainEntityChanged);

        }

        private void LoadProcesses()
        {
            using (var ctx = new GenSoftDBContext())
            {
                if(maxProcessId == 0) maxProcessId = ctx.DomainProcesses.Max(x => x.Id);
                maxProcessId += 1;
                var dp = ctx.DomainProcesses
                    .Include(x => x.Application.DatabaseInfo)
                    .Include(x => x.ParentProcess.DomainProcess.Agent)
                    .Include(x => x.ProcessSteps).ThenInclude(x => x.MainEntity.EntityType.Type)
                    .Include(x => x.ProcessSteps).ThenInclude(x => x.ProcessStepComplexActions).ThenInclude(x => x.ComplexEventAction.ComplexEventActionExpectedEvents).ThenInclude(x => x.ExpectedEvent.EventType.Type)
                    .Include(x => x.ProcessSteps).ThenInclude(x => x.ProcessStepComplexActions).ThenInclude(x => x.ComplexEventAction.ComplexEventActionProcessActions).ThenInclude(x => x.ComplexEventAction.ActionTrigger)
                    .OrderBy(x => x.Priority == 0)
                    .ThenBy(x => x.Priority)
                    .Where(x => x.ProcessSteps.Any(z => z.MainEntity != null))
                    .FirstOrDefault(x => x.ApplicationId == CurrentApplication.Id);
                if (dp == null)
                {
                    SystemProcess systemProcess;
                    
                    var rapp = ctx.Applications
                        .Include(x => x.DatabaseInfo)
                        .First(x => x.Id == CurrentApplication.Id);
                    var dbapp = rapp.DatabaseInfo != null
                        ? new DbApplet(rapp.Name,
                            rapp.DatabaseInfo.ConnectionString)
                        : new Applet(rapp.Name);

                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, Process, "AutoSystemProcess",
                            "AutoSystemProcess", "Auto", Process.User.UserId,dbapp), Process.User, Process.MachineInfo);
                   
                    var parentEntityTypes = ctx.EntityRelationships.Where(x => x.EntityTypeAttribute.EntityType.ApplicationId == CurrentApplication.Id).Select(x => x.ParentEntity.EntityTypeAttribute.EntityType).Distinct();
                    var childEntityTypes = ctx.EntityRelationships.Where(x => x.EntityTypeAttribute.EntityType.ApplicationId == CurrentApplication.Id).Select(x => x.EntityTypeAttribute.EntityType).Distinct();
                    var mainEntities = parentEntityTypes.Where(z => !childEntityTypes.Any(q => q.Id == z.Id)).ToList();
                    mainEntities.AddRange(ctx.EntityTypes.Where(x => x.ApplicationId == CurrentApplication.Id && x.EntityTypeAttributes.All(z => !z.EntityRelationships.Any())));

                    Task.Run(() => { InitializeProcess(systemProcess);}).ConfigureAwait(false);

                    foreach (var mainEntity in mainEntities.DistinctBy(x => x.Id))
                    {
                        Task.Run(() =>
                        {
                            foreach (var complexEvent in GetDBComplexActions(mainEntity.Id, systemProcess))
                            {
                                PublishComplexEvent(systemProcess, complexEvent);
                            }

                        }).ConfigureAwait(false);

                        Task.Run(() =>
                        {
                            foreach (var viewInfo in GetDBViewInfos(mainEntity.Id, false, systemProcess))
                            {
                                PublishViewModel(systemProcess, viewInfo);
                            }

                        }).ConfigureAwait(false);
                    }

                    
                }
                else
                {
                    LoadDomainProcess(dp, Process.User);
                }
            }
        }
       
        private void StartProcess(int objProcessToBeStartedId, IUser user)
        {
            if (CurrentApplication == null) return;
            using (var ctx = new GenSoftDBContext())
            {
                if (maxProcessId == 0) maxProcessId = ctx.DomainProcesses.Max(x => x.Id);

                var dp = ctx.DomainProcesses.OrderBy(x => x.Priority == 0)
                    .ThenBy(x => x.Priority)
                    .Where(x => x.ProcessSteps.Any(z => z.MainEntity != null))
                    .First(x => x.ApplicationId == CurrentApplication.Id && x.Id == objProcessToBeStartedId);

                LoadDomainProcess(dp, user);
            }
        }

        private void StartParentProcess(int processId, IUser user)
        {
            if (CurrentApplication == null) return;
            using (var ctx = new GenSoftDBContext())
            {
                if (maxProcessId == 0) maxProcessId = ctx.DomainProcesses.Max(x => x.Id);

                var dp = ctx.DomainProcesses
                    .Include(x => x.Application.DatabaseInfo)
                    .OrderBy(x => x.Priority == 0)
                    .ThenBy(x => x.Priority)
                    .Where(x => x.ProcessSteps.Any(z => z.MainEntity != null))
                    .First(x => x.ApplicationId == CurrentApplication.Id && x.ParentProcess.Parent_ProcessId == processId);
               
                LoadDomainProcess(dp, user);
            }
        }

        private void LoadDomainProcess(DomainProcess domainProcess, IUser user)
        {
            try
            {

           
            var systemProcess = GetSystemProcess(domainProcess, user);

           
            Task.Run(() => { InitializeProcess(systemProcess); }).ConfigureAwait(false);

            //Parallel.ForEach(domainProcess.ProcessStep,
            //    new ParallelOptions() {MaxDegreeOfParallelism = Processes.ThisMachineInfo.Processors},
            //    (processStep) =>
            //    {
            foreach (var processStep in domainProcess.ProcessSteps)
            {
                if (processStep.MainEntity == null) return;

                    //Parallel.ForEach(processStep.ProcessStepComplexActions,
                    //    new ParallelOptions() {MaxDegreeOfParallelism = Processes.ThisMachineInfo.Processors},
                    //    (pcp) =>
                    //    {
                    foreach (var pcp in processStep.ProcessStepComplexActions)
                    {
                        var cpEvents = new List<IProcessExpectedEvent>();
                        foreach (var ce in pcp.ComplexEventAction.ComplexEventActionExpectedEvents)
                        {
                            cpEvents.Add(CreateProcessExpectedEvent(pcp.ProcessStep.DomainProcessId, ce,
                                processStep.MainEntity.EntityType));
                        }
                        foreach (var cp in pcp.ComplexEventAction.ComplexEventActionProcessActions)
                        {
                            var complexEventAction = CreateComplexEventAction(systemProcess, cp, cpEvents);
                            PublishComplexEvent(systemProcess, complexEventAction);
                        }
                    }
                    //});

                    Task.Run(() =>
                {
                    foreach (var complexEvent in GetDBComplexActions(processStep.MainEntity.EntityType.Id,
                        systemProcess))
                    {
                        PublishComplexEvent(systemProcess, complexEvent);
                    }

                }).ConfigureAwait(false);

                Task.Run(() =>
                {
                    foreach (var viewInfo in GetDBViewInfos(processStep.MainEntity.EntityType.Id, false, systemProcess))
                    {
                        PublishViewModel(systemProcess, viewInfo);
                    }

                }).ConfigureAwait(false);
            }
                // });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private SystemProcess GetSystemProcess(DomainProcess domainProcess, IUser user)
        {
           
                var dbapp = domainProcess.Application.DatabaseInfo != null
                    ? new DbApplet(domainProcess.Application.Name,
                        domainProcess.Application.DatabaseInfo.ConnectionString)
                    : new Applet(domainProcess.Application.Name);

                return new SystemProcess(
                    new SystemProcessInfo(maxProcessId,
                    new SystemProcess(
                            new RevolutionEntities.Process.Process(
                                                                    domainProcess.ParentProcess.Id,
                                                                    Process,
                                                                    domainProcess.ParentProcess.DomainProcess.Name,
                                                                    domainProcess.ParentProcess.DomainProcess.Description,
                                                                    domainProcess.ParentProcess.DomainProcess.Symbol,
                                                                    new RevolutionEntities.Process.Agent(domainProcess.ParentProcess.DomainProcess.Agent.Name),
                                                                    dbapp), Processes.ThisMachineInfo),
                        domainProcess.Name,
                        domainProcess.Description, domainProcess.Symbol,
                        user.UserId, dbapp), user, Process.MachineInfo);
            
        }

        private void InitializeProcess(SystemProcess systemProcess)
        {
            try
            {
                PublishComplexEvent(systemProcess,
                    Processes.ComplexActions.GetComplexAction("StartNextProcess", new object[] {systemProcess}));
                PublishComplexEvent(systemProcess,
                    Processes.ComplexActions.GetComplexAction("ProcessStarted", new object[] {systemProcess}));
                PublishComplexEvent(systemProcess,
                    Processes.ComplexActions.GetComplexAction("CleanUpProcess", new object[] {systemProcess}));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private IProcessExpectedEvent CreateProcessExpectedEvent(int processId, ComplexEventActionExpectedEvent ce, EntityType entityType)
        {
            var eventType = TypeNameExtensions.GetTypeByName(ce.ExpectedEvent.EventType.Type.Name).FirstOrDefault();
            var res = typeof(DomainProcessSupervisor).GetMethod("ProcessExpectedEvent").MakeGenericMethod(eventType)
                .Invoke(null, new object[] {processId, ce, entityType});
            return res as IProcessExpectedEvent;
        }

        public static IProcessExpectedEvent ProcessExpectedEvent<TEventType>(ISystemProcess process, ComplexEventActionExpectedEvent ce,EntityType entityType) where TEventType: IProcessSystemMessage
        {
            Func <TEventType, bool> eventPredicate = CreateExpectedEventPredicate<TEventType>(ce.ExpectedEvent);
            
            return new ProcessExpectedEvent<TEventType>(process: process,
                eventPredicate: eventPredicate,
                processInfo: new RevolutionEntities.Process.StateEventInfo(process,
                    StateEvents[ce.StateEventInfo.StateInfo.Name]),
                expectedSourceType: new RevolutionEntities.Process.SourceType(typeof(IComplexEventService)),
                key: $"{ce.ExpectedEvent.EventType.Type.Name}-{entityType.Type.Name}");
        }

        private static Func<TEventType, bool> CreateExpectedEventPredicate<TEventType>(ExpectedEvent expectedEvents) where TEventType : IProcessSystemMessage
            {
            //e => e.Entity != null && e.Changes.Count == 2 && e.Changes.ContainsKey("Password")
            foreach (var ep in expectedEvents.EventType.EventPredicates)
            {
                var expectedEventPredicateParameters = expectedEvents.ExpectedEventPredicateParameters.Where(x => x.EventPredicateId == ep.Id).ToList();
                var body = CreateExpectedEventPredicateBody(ep.Predicate, expectedEventPredicateParameters);
                var predicateParameters = expectedEventPredicateParameters.Select(x => x.PredicateParameter).ToList();
                var res = DynamicEntityTypeExtensions.CreatePredicate(body, predicateParameters);
                return res;
            }
            return null;
            
        }



        private static string CreateExpectedEventPredicateBody(Predicate predicates, List<ExpectedEventPredicateParameter> predicateParameters)
        {
            var paramlst = new Dictionary<string, string>();
            foreach (var p in predicateParameters)
            {
                if (p.ExpectedEventConstant != null)
                {
                    var c = p.ExpectedEventConstant.Value;
                    paramlst.AddOrUpdate(p.PredicateParameter.Parameter.Name, $"\"{c}\"");
                }

                //var pEntityParameters =
                    //    cp.CalculatedPropertyParameters.Where(x => x.FunctionParameterId == p.Id)
                    //        .DistinctBy(x => x.Id).ToList();
                    //for (int j = 0; j < pEntityParameters.Count(); j++)
                    //{
                    //    var param = pEntityParameters[j];
                    //    var cparameter = param.CalculatedPropertyParameterEntityTypes.DistinctBy(x => x.Id)
                    //        .FirstOrDefault(x => x.CalculatedPropertyParameterId == param.Id)
                    //        ?.EntityTypeAttribute.Attribute.Name;

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
                 var entityType = ctx.EntityTypes.First(x => x.Type.Name == mainEntityChanged.EntityType.Name);
                 var domainProcess = ctx.DomainProcesses
                    .Include(x => x.Application.DatabaseInfo)
                    .Include(x => x.ParentProcess.DomainProcess.Agent)
                    .Include(x => x.ProcessSteps).ThenInclude(x => x.MainEntity.EntityType)
                    .OrderBy(x => x.Priority == 0).ThenBy(x => x.Priority)
                    .FirstOrDefault(x => x.ProcessSteps.Any(z => z.MainEntity.EntityType.Id == entityType.Id));
                SystemProcess systemProcess;
                _processComplexEvents.Clear();
                maxProcessId += 1;
                if (domainProcess == null)
                {
                    var rapp = ctx.Applications
                        .Include(x => x.DatabaseInfo)
                        .First(x => x.Id == CurrentApplication.Id);
                    var dbapp = rapp.DatabaseInfo != null
                        ? new DbApplet(rapp.Name,
                            rapp.DatabaseInfo.ConnectionString)
                        : new Applet(rapp.Name);

                    systemProcess = new SystemProcess(
                        new SystemProcessInfo(maxProcessId, mainEntityChanged.Process, $"AutoProcess-{entityType.EntitySet}",
                            $"AutoProcess-{entityType.EntitySet}", "Auto", mainEntityChanged.User.UserId, dbapp), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                }
                else
                {
                    var dbapp = domainProcess.Application.DatabaseInfo != null
                        ? new DbApplet(domainProcess.Application.Name,
                            domainProcess.Application.DatabaseInfo.ConnectionString)
                        : new Applet(domainProcess.Application.Name);
                    systemProcess = new SystemProcess(
                    new SystemProcessInfo(maxProcessId, mainEntityChanged.Process, domainProcess.Name,
                        domainProcess.Description, domainProcess.Symbol, mainEntityChanged.User.UserId,dbapp), mainEntityChanged.User, mainEntityChanged.MachineInfo);
                }
                Task.Run(() => { InitializeProcess(systemProcess);}).ConfigureAwait(false);
                
                Task.Run(() =>
                {
                    foreach (var complexEvent in GetDBComplexActions(entityType.Id, systemProcess))
                    {
                        PublishComplexEvent(systemProcess, complexEvent);
                    }
                }).ConfigureAwait(false);

                Task.Run(() =>
                {
                    foreach (var viewModel in GetDBViewInfos(entityType.Id, false, systemProcess))
                    {
                        PublishViewModel(systemProcess, viewModel);
                    }

                }).ConfigureAwait(false);
            }
        }

        private ConcurrentDictionary<string,string> _processComplexEvents = new ConcurrentDictionary<string, string>();

        private void PublishComplexEvent(SystemProcess systemProcess, IComplexEventAction processComplexEvent)
        {
            if (_processComplexEvents.TryAdd(processComplexEvent.Key, processComplexEvent.Key) == false) return;
            try
            {
                var inMsg = new LoadProcessComplexEvents(new List<IComplexEventAction>() {processComplexEvent},
                    new RevolutionEntities.Process.StateCommandInfo(systemProcess,
                        RevolutionData.Context.CommandFunctions.UpdateCommandData(processComplexEvent.Key,
                            RevolutionData.Context.Process.Commands.StartProcess)),
                    systemProcess, Source);

                EventMessageBus.Current.Publish(inMsg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        //private void PublishViewModels(SystemProcess systemProcess, List<IViewModelInfo> viewModelInfos)
        //{
        //    var inMsg = new LoadDomainProcessViewModels(viewModelInfos,
        //        new RevolutionEntities.Process.StateCommandInfo(systemProcess, RevolutionData.Context.Process.Commands.StartProcess),
        //        systemProcess, Source);

        //    EventMessageBus.Current.Publish(inMsg, Source);
        //}
        private void PublishViewModel(SystemProcess systemProcess, IViewModelInfo viewModelInfo)
        {
            var entityViewInfo = viewModelInfo.ViewInfo as IEntityViewInfo;

            var inMsg = new LoadDomainProcessViewModels(new List<IViewModelInfo>(){viewModelInfo},
                new RevolutionEntities.Process.StateCommandInfo(systemProcess, RevolutionData.Context.CommandFunctions.UpdateCommandData(entityViewInfo.EntityType.Name, RevolutionData.Context.Process.Commands.StartProcess)),
                systemProcess, Source);

            EventMessageBus.Current.Publish(inMsg);
        }

        private void BuildExpressions()
        {
            BuildFunctions();
           // BuildActions();
            BuildStateCommandInfo();
            BuildStateEventInfo();
            //BuildProcessActions();
      
        }


        private ComplexEventAction CreateComplexEventAction(ISystemProcess process, ComplexEventActionProcessAction complexEventAction, IList<IProcessExpectedEvent> cpEvents)
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

        private Func<IDynamicComplexEventParameters, Task<dynamic>> CreateComplexEventParametersAction(
            Action action)
        {
            try
            {
                var actions = new List<Func<IDynamicComplexEventParameters, dynamic>>();

                
                    var interpreter = new Interpreter();
                    foreach (var r in action.ActionReferenceTypes)
                    {
                        if (r.ReferenceType.ReferenceTypeName != null)
                        {
                            interpreter.Reference(new ReferenceType(r.ReferenceType.ReferenceTypeName.Name,
                                TypeNameExtensions.GetTypeByName(r.ReferenceType.DataType.Type.Name)[0]));
                        }
                        else
                        {
                            interpreter.Reference(
                                TypeNameExtensions.GetTypeByName(r.ReferenceType.DataType.Type.Name)[0]);
                        }
                    }
                interpreter.Reference(typeof(SystemProcess));
                
                var body = action.Body;
                var res = interpreter.ParseAsDelegate<Func<IDynamicComplexEventParameters, IProcessSystemMessage>>(body, action.Parameter);
               var restuple = new Tuple<Action, Func<IDynamicComplexEventParameters, IProcessSystemMessage>>(action, res);
             
                return async cp => await Task.Run(() => restuple.Item2(cp)).ConfigureAwait(false);
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
                    var lst = ctx.StateEventInfos
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

            var stateEvent = new StateEvent(f.StateInfo.Name, f.StateInfo.Status, f.StateInfo.StateInfoNote?.Notes,f.StateInfo.Subject,"Unknown", command);
            StateEvents.AddOrUpdate(f.StateInfo.Name, stateEvent);
            return stateEvent;
        }

        private void BuildStateCommandInfo()
        {
            try
            {
                using (var ctx = new GenSoftDBContext())
                {
                    var lst = ctx.StateCommandInfos
                        .Include(x => x.StateInfo)
                        .Include(x => x.ExpectedStateEventInfo.StateEventInfo.StateInfo)
                        .ToList();

                    foreach (var f in lst)
                    {

                        var res = new StateCommand(f.StateInfo.Name, f.StateInfo.Status,f.StateInfo.Subject, "Unknown");
                        if (f.ExpectedStateEventInfo?.StateEventInfo != null)
                        {
                            var se = CreateStateEvent(f.ExpectedStateEventInfo.StateEventInfo, res);
                            res.ExpectedEvent = se;
                        }
                        StateCommands.AddOrUpdate(f.StateInfo.Name, res);
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
                var lst = ctx.Actions
                    .Include(x => x.ActionParameters).ThenInclude(x => x.ActionPropertyParameters)
                    .ToList();

                foreach (var f in lst)
                {
                    var actionParameters = f.ActionParameters.ToList();

                    var res = CreateAction(f.Body, actionParameters);
                    DynamicEntityTypeExtensions.Functions.Add(f.Name, res);
                }
            }
        }

        private static dynamic CreateAction(string body, List<ActionParameter> actionParameters)
        {
            throw new NotImplementedException();
        }

        private static void BuildFunctions()
        {
            using (var ctx = new GenSoftDBContext())
            {
                var lst = ctx.Functions
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.FunctionParameterConstants)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperty).ThenInclude(x => x.EntityTypeAttribute.Attribute)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperty).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.FunctionParameter)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperty).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedPropertyParameterEntityTypes)
                    .ThenInclude(x => x.EntityTypeAttribute.Attribute)
                    .Include(x => x.FunctionParameters).ThenInclude(x => x.CalculatedPropertyParameters)
                    .ThenInclude(x => x.CalculatedProperty).ThenInclude(x => x.FunctionParameterConstants)
                    .Where(x => x.FunctionParameters.All(z =>
                        !z.CalculatedPropertyParameters.Any() && !z.FunctionParameterConstants.Any()))
                    .ToList();

                foreach (var f in lst)
                {
                    var FunctionParameter = f.FunctionParameters
                        .DistinctBy(x => x.Id).ToList();

                    var res = DynamicEntityTypeExtensions.CreateFunction(f.Body, FunctionParameter, f.ReturnDataTypeId);
                    DynamicEntityTypeExtensions.Functions.Add(f.Name, res);
                }
            }
        }
     






        public static IEnumerable<IViewModelInfo> GetDBViewInfos(int mainEntityId, bool condensed, ISystemProcess process)
        {
           
            
            using (var ctx = new GenSoftDBContext())
            {
                var list = ctx.EntityRelationships
                    .Include(x => x.EntityTypeAttribute.EntityType.Type)
                    .Include(x => x.EntityTypeAttribute.EntityType).ThenInclude(x => x.EntityTypeViewModelCommands).ThenInclude(x => x.ViewModelCommand.CommandType)
                    .Include(x => x.RelationshipType.ChildOrdinality)
                    .Include(x => x.RelationshipType.ParentOrdinality)
                    .Include(x => x.EntityTypeAttribute.Attribute)
                    .Include(x => x.ParentEntity.EntityTypeAttribute.EntityType.Type)
                    .Include(x => x.ParentEntity.EntityTypeAttribute.EntityType).ThenInclude(x => x.EntityTypeViewModelCommands).ThenInclude(x => x.ViewModelCommand.CommandType)
                    .Include(x => x.ParentEntity.EntityTypeAttribute.Attribute)
                    .Where(x => x.ParentEntity.EntityTypeAttribute.EntityType.Id == mainEntityId || x.EntityTypeAttribute.EntityType.Id == mainEntityId)
                    .GroupBy(x => x.ParentEntity.EntityTypeAttribute.EntityType).ToList();

                foreach (var g in list)
                {
                    var pm = CreateEntityTypeViewModel(g.Key, new List<EntityRelationship>(), true, ctx, process, EntityRelationshipOrdinality.One);
                    if (pm == null) continue;
                    
                    var ppm = CreateEntityTypeViewModel(g.Key, new List<EntityRelationship>(), false, ctx, process, EntityRelationshipOrdinality.One);
                    var ppv = CreateEntityViewModel(ppm, new List<IViewModelInfo>());
                    var childviews = new List<IViewModelInfo>(){ppv};
                    foreach (var rel in g)
                    {
                        ///////////////////////////////////////////////// EF No loading the includes for EntityTypeAttributes.EntityType ///////////////////////
                        var relEntity = ctx.EntityTypes
                            .Include(x => x.Type)
                            .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                            .Include(x => x.EntityTypeViewModelCommands)
                            .ThenInclude(x => x.ViewModelCommand.CommandType)
                            .First(x => x.Id == rel.EntityTypeAttribute.EntityType.Id);


                        var cm = CreateEntityTypeViewModel(relEntity, new List<EntityRelationship>() { rel }, rel.RelationshipType.ChildOrdinalityId == 2, ctx, process, rel.RelationshipType.ChildOrdinality.Name == "One" ? EntityRelationshipOrdinality.One : EntityRelationshipOrdinality.Many);
                        var cv = CreateEntityViewModel(cm, new List<IViewModelInfo>());
                        if (cv != null)
                        {
                            if (rel.RelationshipType.ChildOrdinality.Name == "One" || condensed)
                            {
                                cv.Visibility = Visibility.Visible;
                                childviews.Add(cv);
                            }
                            else
                            {
                                cv.Visibility = Visibility.Collapsed;
                                childviews.Add(cv);
                                yield return cv;
                            }


                        }

                    }
                    var pv = CreateEntityViewModel(pm, childviews);
                    yield return pv;
                }
                if (list.Any()) yield break;
                {
                    var mainEntity = ctx.EntityTypes
                        .Include(x => x.Type)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                        .Include(x => x.EntityTypeViewModelCommands).ThenInclude(x => x.ViewModelCommand.CommandType)
                        .FirstOrDefault(x => x.Id == mainEntityId && x.EntityTypeAttributes.Any(z => z.EntityRelationships.Any()));
                    if (mainEntity != null)
                    {
                        var mppm = CreateEntityTypeViewModel(mainEntity, new List<EntityRelationship>(), false, ctx,
                            process, EntityRelationshipOrdinality.One);
                        var mppv = CreateEntityViewModel(mppm, new List<IViewModelInfo>());
                        yield return mppv;
                    }
                }

                
                    var soleEntity = ctx.EntityTypes
                        .Include(x => x.Type)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                        .Include(x => x.EntityTypeViewModelCommands).ThenInclude(x => x.ViewModelCommand.CommandType)
                        .FirstOrDefault(x => x.Id == mainEntityId && x.EntityTypeAttributes.All(z => !z.EntityRelationships.Any()));
                if (soleEntity != null)
                {
                    var sppm1 = CreateEntityTypeViewModel(soleEntity, new List<EntityRelationship>(), false, ctx,
                        process, EntityRelationshipOrdinality.One);
                    var sppv1 = CreateEntityViewModel(sppm1, new List<IViewModelInfo>());
                    var sppm = CreateEntityTypeViewModel(soleEntity, new List<EntityRelationship>(), true, ctx,
                        process, EntityRelationshipOrdinality.One);
                    var sppv = CreateEntityViewModel(sppm, new List<IViewModelInfo>(){sppv1});
                    
                    
                    yield return sppv;
                }

            }
        }

        private static EntityTypeViewModel CreateEntityTypeViewModel(EntityType entityType, List<EntityRelationship> relationships, bool isList, GenSoftDBContext ctx, ISystemProcess process, EntityRelationshipOrdinality ordinality)
        {
            return new EntityTypeViewModel()
            {
                Description = entityType.EntitySet,
                SystemProcess = process,
                EntityTypeName = entityType.Type.Name,
                RelationshipOrdinality = ordinality,
                ViewModelTypeName = ctx.ViewModelTypes.First(z => z.DomainEntity == true && z.List == isList).Name,
                EntityTypeViewModelCommands = entityType.EntityTypeViewModelCommands.ToList(),
                EntityViewModelRelationships = relationships.Select(x => new EntityViewModelRelationship()
                {
                    ParentType = x.ParentEntity.EntityTypeAttribute.EntityType.Type.Name,
                    ParentProperty = x.ParentEntity.EntityTypeAttribute.Attribute.Name,
                    ChildType = x.EntityTypeAttribute.EntityType.Type.Name,
                    ChildProperty = x.EntityTypeAttribute.Attribute.Name,

                }).ToList()
            };
        }

        

        private static IViewModelInfo CreateEntityViewModel(EntityTypeViewModel vm, List<IViewModelInfo> childviews)
        {
           
            var vp = CreateViewAttributeDisplayProperties(vm);
            var res = ProcessViewModels.ProcessViewModelFactory[vm.ViewModelTypeName].Invoke(vm,childviews, vp);
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

                var basicTheme = ctx.ViewPropertyThemes
                    .Include(x => x.ViewPropertyValueOption)
                    .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                    .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                    .Where(x => x.ViewType.Name == viewType)
                    .Select(x => new
                    {
                        PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name,
                        ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                        x.ViewPropertyValueOption.Value
                    }).GroupBy(x => x.PresentationPropertyName)
                    .Select(x => new { x.Key, Value = x.ToDictionary(z => z.ViewPropertyName, z => z.Value) }).ToDictionary(x => x.Key, x => x.Value);

                var viewTypeTheme =
                    ctx.ViewModelPropertyPresentationTypes
                        .Include(x => x.ViewPropertyValueOption)
                        .Include(x => x.ViewPropertyPresentationPropertyType.PresentationPropertyType)
                        .Include(x => x.ViewPropertyPresentationPropertyType.ViewProperty)
                        .Where(x => x.ViewType.Name == viewType && x.ViewModelType.Name == vm.ViewModelTypeName)
                        .Select(x => new
                        {
                            PresentationPropertyName = x.ViewPropertyPresentationPropertyType.PresentationPropertyType.Name,
                            ViewPropertyName = x.ViewPropertyPresentationPropertyType.ViewProperty.Name,
                            x.ViewPropertyValueOption.Value
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

        public IEnumerable<IComplexEventAction> GetDBComplexActions(int mainEntityId, ISystemProcess process)
        {
            
              //var res = new ConcurrentDictionary<string,IComplexEventAction>();


                using (var ctx = new GenSoftDBContext())
                {
                    
                    var list = ctx.EntityRelationships
                        .Include(x => x.EntityTypeAttribute.EntityType.Type)
                        .Include(x => x.RelationshipType.ChildOrdinality)
                        .Include(x => x.RelationshipType.ParentOrdinality)
                        .Include(x => x.EntityTypeAttribute.Attribute)
                        .Include(x => x.ParentEntity.EntityTypeAttribute.EntityType.Type)
                        .Include(x => x.ParentEntity.EntityTypeAttribute.Attribute)
                        .Where(x => x.ParentEntity.EntityTypeAttribute.EntityType.Id == mainEntityId || x.EntityTypeAttribute.EntityType.Id == mainEntityId)
                        .GroupBy(x => x.ParentEntity.EntityTypeAttribute.EntityType);

                    foreach (var r in list)
                    {
                        var parentType = r.Key.Type.Name;
                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType);


                        yield return Processes.ComplexActions.GetComplexAction("InitializeProcessStateList",
                            new object[] {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType)});
                        yield return Processes.ComplexActions.GetComplexAction("UpdateStateList",
                            new object[] {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType)});
                        yield return Processes.ComplexActions.GetComplexAction("UpdateState",
                            new object[] {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType)});
                        yield return Processes.ComplexActions.GetComplexAction("RequestState",
                            new object[] {process, parentType, parentType, "Id"});

                        var entityRelationships = r.DistinctBy(x => x.Id);
                        foreach (var rel in entityRelationships)
                        {
                            var parentExpression = rel.ParentEntity.EntityTypeAttribute.Attribute.Name;
                            var childType = rel.EntityTypeAttribute.EntityType.Type.Name;

                            //if (DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType) != null)
                            //{
                            //    DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType);
                            //    if (rel.RelationshipType.ChildOrdinality.Name != "One")
                            //    {
                            //        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType).ChildEntities
                            //            .Add(new DynamicRelationshipType(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType),rel.EntityTypeAttribute.Attribute.Name));
                            //        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType).ParentEntities
                            //            .Add(new DynamicRelationshipType(DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(parentType), rel.ParentEntity.EntityTypeAttribute.Attribute.Name));
                            //    }
                            //}



                            var childExpression = rel.EntityTypeAttribute.Attribute.Name;


                            if (rel.RelationshipType.ChildOrdinality.Name == "One")
                            {
                                yield return Processes.ComplexActions.GetComplexAction("UpdateState",
                                    new object[]
                                        {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType)});

                                yield return Processes.ComplexActions.GetComplexAction("RequestState",
                                    new object[] {process, parentType, childType, childExpression});
                            }
                            else
                            {
                                yield return Processes.ComplexActions.GetComplexAction("UpdateStateList",
                                    new object[]
                                        {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(childType)});

                                yield return Processes.ComplexActions.GetComplexAction("RequestStateList",
                                    new object[] {process, parentType, childType, parentExpression, childExpression});
                            }
                            yield return Processes.ComplexActions.GetComplexAction("UpdateStateWhenDataChanges",
                                new object[] {process, parentType, childType, parentExpression, childExpression});

                        }

                    }
                    if (list.Any()) yield break;
                   
                        var mainEntity = ctx.EntityTypes
                            .Include(x => x.Type)
                            .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                            .FirstOrDefault(x => x.Id == mainEntityId && x.EntityTypeAttributes.Any(z => z.EntityRelationships.Any()));// just for signin
                    if (mainEntity != null)
                    {
                        var mainEntityType = mainEntity.Type.Name;
                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType);

                    yield return Processes.ComplexActions.GetComplexAction("InitializeProcessState",
                        new object[]
                            {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType)});
                    yield return Processes.ComplexActions.GetComplexAction("UpdateState",
                        new object[]
                            {process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(mainEntityType)});
                    yield return Processes.ComplexActions.GetComplexAction("RequestState",
                        new object[] { process, mainEntityType, mainEntityType, "Id" });
                    yield break;
                    }

                    var soleEntity = ctx.EntityTypes
                        .Include(x => x.Type)
                        .Include(x => x.EntityTypeAttributes).ThenInclude(x => x.Attribute)
                        .FirstOrDefault(x => x.Id == mainEntityId && x.EntityTypeAttributes.All(z => !z.EntityRelationships.Any()));
                    if (soleEntity != null)
                    {
                  
                   yield return Processes.ComplexActions.GetComplexAction("InitializeProcessStateList", new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(soleEntity.Type.Name) });
                        yield return Processes.ComplexActions.GetComplexAction("UpdateStateList", new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(soleEntity.Type.Name) });

                    yield return Processes.ComplexActions.GetComplexAction("UpdateState", new object[] { process, DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(soleEntity.Type.Name) });
                    yield return Processes.ComplexActions.GetComplexAction("RequestState", new object[] { process, soleEntity.Type.Name, soleEntity.Type.Name, "Id" });


                }

            }
        }



    }


}