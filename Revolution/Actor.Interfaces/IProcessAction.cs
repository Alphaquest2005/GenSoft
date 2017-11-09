﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;

namespace Actor.Interfaces
{
    
    public interface IProcessAction
    {
        Func<IDynamicComplexEventParameters, Task<IProcessSystemMessage>> Action { get; set; }
        Func<IDynamicComplexEventParameters, IProcessStateInfo> ProcessInfo { get; set; }
        ISourceType ExpectedSourceType { get; set; }
    }
}
