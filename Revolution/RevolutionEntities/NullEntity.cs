using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using Common;
using Common.DataEntites;
using RevolutionEntities.Process;

namespace RevolutionEntities
{
    public sealed class NullEntity : DynamicEntity
    {

        //public ISystemSource Source => new Source(Guid.NewGuid(), "NullEntity" + typeof(NullEntity<T>).Name, new SourceType(typeof(NullEntity<T>)),new SystemProcess(new Process.Process(1,0, "Starting System", "Prepare system for Intial Use","", new Agent("System")), new MachineInfo(Environment.MachineName, Environment.ProcessorCount)), new MachineInfo(Environment.MachineName, Environment.ProcessorCount));
        public NullEntity(string entityType) : base(entityType, -1)
        {
        }
    }


}

