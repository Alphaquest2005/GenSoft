﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using SystemInterfaces;
using CommonMessages;

namespace EventMessages.Commands
{
    [Export(typeof(IAddOrGetEntityWithChanges))]

    public class AddOrGetEntityWithChanges : ProcessSystemMessage, IAddOrGetEntityWithChanges
    {
        public AddOrGetEntityWithChanges()
        {
           
        }
        public Dictionary<string, dynamic> Changes { get; }
        
        public AddOrGetEntityWithChanges(string entityType, Dictionary<string, dynamic> changes, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(processInfo, process, source)
        {
            Contract.Requires(changes.Count > 0);
            Changes = changes;
            EntityType = entityType;
        }

        public string EntityType { get; }
    }
}