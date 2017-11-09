using System;
using System.ComponentModel.Composition;
using SystemInterfaces;

namespace CommonMessages
{
    [Export(typeof(ISystemMessage))]
    public class SystemMessage :ISystemMessage
    {
        public SystemMessage()
        {
            
        }
        public SystemMessage(IDynamicObject message, IMachineInfo machineInfo, ISystemSource source)
        {
            Source = source;
            MessageDateTime = DateTime.Now;
            MachineInfo = machineInfo;
            Message = message;
        }

        public ISystemSource Source { get; }
        public IDynamicObject Message { get; }
        public DateTime MessageDateTime { get; }
        public IMachineInfo MachineInfo { get; }
    }
}
