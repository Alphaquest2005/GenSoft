using SystemInterfaces;

namespace RevolutionEntities.Process
{
    public struct State : IState
    {
        public State(  string name, string status, string notes, string subject, string data)
        {
            Status = status;
            Name = name;
            Notes = notes;
            Subject = subject;
            Data = data;
        }

        public string Name { get; }
        public string Status { get; }
        public string Notes { get; }
        public string Subject { get; }
        public string Data { get; }
    }

    public struct StateEvent : IStateEvent
    {
        public StateEvent(string name, string status, string notes, string subject, string data, IStateCommand expectedCommand = null) 
        {
            ExpectedCommand = expectedCommand;
            Status = status;
            Name = name;
            Notes = notes;
            Subject = subject;
            Data = data;
        }

        public IStateCommand ExpectedCommand { get; }
        public string Name { get; }
        public string Status { get; }
        public string Notes { get; }
        public string Subject { get; }
        public string Data { get; }
    }

    public struct StateCommand : IStateCommand
    {
       
        public StateCommand(string name, string status, string subject, string data, IStateEvent expectedEvent = null)
        {
            ExpectedEvent = expectedEvent;
            Status = status;
            Subject = subject;
            Data = data;
            Name = name;
            Notes = "";
        }

        public IStateEvent ExpectedEvent { get; set; }
        public string Name { get; }
        public string Status { get; }
        public string Notes { get; }
        public string Subject { get; }
        public string Data { get; }
    }
}