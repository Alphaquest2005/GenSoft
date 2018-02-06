using System;
using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public static class CommandFunctions
    {
        public static Func<string, IStateCommand, IStateCommand> UpdateCommandStatus =>
            (s, c) => new StateCommand(c.Name, $"{c.Status}-{s}", c.ExpectedEvent);
    }

    public static class EventFunctions
    {
        public static Func<string, IStateEvent, IStateEvent> UpdateEventStatus => (s, c) =>
            new StateEvent(c.Name, $"{c.Status}-{s}", c.Notes, c.ExpectedCommand);
    }
}
