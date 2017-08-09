using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IProcessStateMessage:IEntityRequest
    {
        IProcessStateEntity State { get; }
    }

    
    public interface IUpdateProcessStateList : IEntityRequest 
    {
        IProcessStateList State { get; }
    }
}
