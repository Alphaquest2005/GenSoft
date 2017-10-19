﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using Actor.Interfaces;
using GenSoft.Entities;
using GenSoft.Interfaces;
using RevolutionEntities.Process;
using ComplexEventAction = RevolutionEntities.Process.ComplexEventAction;
using IProcessStateInfo = SystemInterfaces.IProcessStateInfo;
using ISourceType = SystemInterfaces.ISourceType;
using IStateCommandInfo = SystemInterfaces.IStateCommandInfo;
using ProcessAction = RevolutionEntities.Process.ProcessAction;
using SourceType = RevolutionEntities.Process.SourceType;
using StateCommandInfo = RevolutionEntities.Process.StateCommandInfo;
using SystemProcess = GenSoft.Entities.SystemProcess;
using Type = System.Type;

namespace GenSoft.Expressions
{
    public class ComplexEventActionData
    {
        public string Name { get; set; }
        public int ProcessId { get; set; }
        public string ExpectedMessageType { get; set; }
        public ProcessActionData ProcessAction { get; set; }
        public IProcessStateInfo ActionTrigger
        { get; set; }
    }


    public class EventData
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public EventData ExpectedEventCommand { get; set; }
    }

    public class ProcessActionData
    {
        public SystemProcessInfo SystemProcessInfo { get; set; }
        public TypeData ExpectedSourceType { get; set; }
        public MessageData MessageData { get; set; }
        public MessageData CommandData { get; set; }
    }

    public class TypeData
    {
        public string TypeString { get; set; }
    }

    public class MessageData
    {
        public string MessageType { get; set; }
        public EventData Event { get; set; }
    }

    public class ProcessExpressions
    {
        public static Func<ComplexEventActionData, ComplexEventAction> CreateComplexEventAction =
    (cd) => new ComplexEventAction(cd.Name,
                                   cd.ProcessId,
                                   new List<IProcessExpectedEvent>(),
                                   Type.GetType(cd.ExpectedMessageType),
                                   CreateProcessAction.Invoke(cd.ProcessAction),
                                   cd.ActionTrigger);

        public static Func<ProcessActionData, ProcessAction> CreateProcessAction
            = (pd) => new ProcessAction(CreateActionFromComplexEvent.Invoke(pd.MessageData),
                                        CreateProcessInfoFromComplexEventData.Invoke(pd.CommandData),
                                        CreateSourceType.Invoke(pd.ExpectedSourceType));

        public static Func<MessageData, Func<IComplexEventParameters, Task<IProcessSystemMessage>>>
            CreateActionFromComplexEvent =
                (cpd) => async cp => await Task.Run(() => CreateEvent.Invoke(cpd, cp));

        public static Func<MessageData, IComplexEventParameters, IProcessSystemMessage> CreateEvent
            = (md, cp) =>
            {
                var type = Type.GetType(md.MessageType);
                return (IProcessSystemMessage)Activator.CreateInstance(type, new object[] {new RevolutionEntities.Process.StateEventInfo(cp.Actor.Process.Id, new StateEvent(md.Event.Name, md.Event.Status, md.Event.Notes, CreateStateCommand.Invoke(md.Event.ExpectedEventCommand))),
                       cp.Actor.Process, cp.Actor.Source});

            };

        public static Func<MessageData, IComplexEventParameters, IStateCommandInfo> CreateCommand
            = (md, cp) => new StateCommandInfo(cp.Actor.Process.Id, new StateCommand(md.Event.Name, md.Event.Status, CreateStateEvent(md.Event)));

        public static Func<EventData, IStateEvent> CreateStateEvent = (ed) => new StateEvent(ed.Name, ed.Status, ed.Notes, CreateStateCommand(ed.ExpectedEventCommand));

        public static Func<EventData, IStateCommand> CreateStateCommand = (ed) => new StateCommand(ed.Name, ed.Status, CreateStateEvent.Invoke(ed.ExpectedEventCommand));

        public static Func<MessageData, Func<IComplexEventParameters, IStateCommandInfo>>
            CreateProcessInfoFromComplexEventData = (md) => cp => CreateCommand.Invoke(md, cp);

        public static Func<TypeData, ISourceType> CreateSourceType = (td) => new SourceType(Type.GetType(td.TypeString));

        public static Func<SystemProcess, SystemInterfaces.ISystemProcessInfo> CreateProcessInfo = (p) => new SystemProcessInfo(p.Id, p.ParentProcessId, p.Name, p.Description, p.Symbol, p.UserId.ToString()) as SystemInterfaces.ISystemProcessInfo;

        public static Func<Machine, IMachineInfo> CreateMachineInfo = (m) => new MachineInfo(m.MachineName, m.Processors);
    }
}
