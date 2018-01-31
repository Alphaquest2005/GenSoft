using System;
using System.Collections.Generic;
using SystemInterfaces;
using Actor.Interfaces;

namespace RevolutionEntities.Process
{
    public class ComplexEventAction: IComplexEventAction
    {
        public ComplexEventAction() { }
        public ComplexEventAction(string key, ISystemProcess process, IList<IProcessExpectedEvent> events, Type expectedMessageType, IProcessAction action, IProcessStateInfo processInfo, ActionTrigger actionTrigger = ActionTrigger.All)//Todo:get rid of optional parameters
        {
            Process = process;
            Events = events;
            ExpectedMessageType = expectedMessageType;
            ProcessInfo = processInfo;
            ActionTrigger = actionTrigger;
            Key = key;
            Action = action;
        }

        public string Key { get; }
        public IList<IProcessExpectedEvent> Events { get; }
        public ISystemProcess Process { get; }
        public Type ExpectedMessageType { get; }
        public IProcessStateInfo ProcessInfo { get; }
        public IProcessAction Action { get; }
        public ActionTrigger ActionTrigger { get; }
    }

    public static class ComplexEventActionExtensions
    {
      
       
    }
}