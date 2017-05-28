using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    
    public interface IEntitySetWithFilterLoaded<TEntity> : IMessage where TEntity : IEntity
    {
        IList<TEntity> Entities { get; }
    }
}
