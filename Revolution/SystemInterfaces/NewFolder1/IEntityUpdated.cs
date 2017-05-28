﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInterfaces
{
    public interface IEntityUpdated<out TEntity> : IMessage where TEntity : IEntityId
    {
        TEntity Entity { get; }
    }
}
