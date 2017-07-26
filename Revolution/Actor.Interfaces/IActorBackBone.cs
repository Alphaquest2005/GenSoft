using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    
    public interface IActorBackBone
    {
        void Intialize(bool autoRun, List<IMachineInfo> machineInfo, List<IProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos, Assembly interfaces);
        void Intialize(bool autoContinue, List<IViewModelInfo> viewInfos);
    }
}
