using System;
using System.ComponentModel.Composition;
using SystemInterfaces;

namespace CommonMessages
{
    [Export(typeof(IMessage))]
    public class Message :IMessage
    {
        //public Message()
        //{
           
        //}

        public Message(int messageSourceId, int machineId, int processId)
        {
            MessageSourceId = messageSourceId;
            MachineId = machineId;
            ProcessId = processId;
        }


        public int MessageSourceId { get; }
        public int MachineId { get; }
        public int ProcessId { get; }
        public DateTime EntryDateTime { get; } = DateTime.Now;
    }
}
