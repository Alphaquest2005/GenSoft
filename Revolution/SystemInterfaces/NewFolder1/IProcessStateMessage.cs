using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IProcessStateMessage<out TEntity>:IMessage where TEntity : IEntityId
    {
        IProcessState<TEntity> State { get; }
    }
}
