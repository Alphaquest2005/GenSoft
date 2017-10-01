﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    
    public interface IActorBackBone
    {
        void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<ISystemProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos);
        void Intialize(bool autoContinue, List<IViewModelInfo> viewInfos);
        void Intialize(bool autoContinue, List<IComplexEventAction> processComplexEvents, List<IViewModelInfo> processViewModelInfos);
    }
}
