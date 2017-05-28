using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntityWithChangesFound<TEntity>:IMessage where TEntity : IEntity
    {
        TEntity Entity { get; set; }
        Dictionary<string, object> Changes { get; }
    }
}
