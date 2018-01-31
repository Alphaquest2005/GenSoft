using System.ComponentModel.Composition;
using SystemInterfaces;

namespace CommonMessages
{
    [Export(typeof(IProcessSystemMessage))]
    public class ProcessSystemMessage : SystemMessage, IProcessSystemMessage
    {
        public ProcessSystemMessage() {}

        public ProcessSystemMessage(IDynamicObject message, IProcessStateInfo processInfo, ISystemProcess process, ISystemSource source) : base(message, source.MachineInfo,source)
        {
            Process = process;
            ProcessInfo = processInfo;
            ParentProcess = Process.ParentProcess;
            Name = process.Name;
            Description = process.Description;
            Symbol = process.Symbol;
            User = process.User;
            Id = process.Id;
        }

        public int Id { get; }
        public ISystemProcess ParentProcess { get; } 
        public string Name { get; }
        public string Description { get; }
        public string Symbol { get; }
        public IUser User { get; }
        public ISystemProcess Process { get; }
        public IProcessStateInfo ProcessInfo { get; }
    }
}