using System.Collections.Generic;

namespace SystemInterfaces
{
    
    public interface IProcessState
    {
        int ProcessId { get; }
        IProcessStateInfo StateInfo { get; }

        ISystemProcess Process { get; }

    }

    
    public interface IProcessStateEntity: IProcessState 
    {
       IDynamicEntity Entity { get; set; }
    }

    
    public interface IProcessStateList : IProcessState
    {
        IEnumerable<IDynamicEntity> EntitySet { get; }
        IEnumerable<IDynamicEntity> SelectedEntities { get; }
        
    }

}