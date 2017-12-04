using System;

namespace RevolutionEntities.Process
{
    public class ProcessExpectedEventInfo
    {
        public Type EventType { get; set; }
        public ProcessStateInfo ProcessInfo { get; set; }
        public SourceType ExpectedSourceType { get; set; }

        public ProcessExpectedEventInfo(Type eventType, ProcessStateInfo processInfo, SourceType expectedSourceType)
        {
            EventType = eventType;
            ProcessInfo = processInfo;
            ExpectedSourceType = expectedSourceType;
        }
    }
}
