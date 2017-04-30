using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    public interface IEntityFound<out TEntity> : IProcessSystemMessage where TEntity : IEntityId
    {
        TEntity Entity { get; }
    }
}
