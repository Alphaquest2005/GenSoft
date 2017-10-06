﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface ICreateProcessActor : IProcessSystemMessage
    {
        string ActorName { get; }
        IList<IComplexEventAction> ComplexEvents { get; }
    }
}
