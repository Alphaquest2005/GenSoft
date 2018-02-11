using System;
using SystemInterfaces;
using RevolutionEntities.Process;

namespace RevolutionData.Context
{
    public static class CommandFunctions
    {
        public static Func<string, IStateCommand, IStateCommand> UpdateCommandData =>
            (s, c) => new StateCommand(c.Name, c.Status, c.Subject, s, c.ExpectedEvent);
    }

    public static class EventFunctions
    {
        public static Func<string, IStateEvent, IStateEvent> UpdateEventData => (s, c) =>
            new StateEvent(c.Name, c.Status, c.Notes, c.Subject, s, c.ExpectedCommand);
    }
}
