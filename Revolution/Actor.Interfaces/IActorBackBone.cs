using System.Collections.Generic;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    
    public interface IActorBackBone
    {
        void Initialize(bool autoRun, List<IMachineInfo> machineInfo, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewInfos);
        void Initialize(bool autoContinue, List<IViewModelInfo> viewInfos);
        void Initialize(bool autoContinue, List<IComplexEventAction> processComplexEvents, List<IViewModelInfo> processViewModelInfos);
    }
}
