using System.Collections.Generic;
using SystemInterfaces;
using GenSoft.Interfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    public interface ILoadDomainProcess : IProcessSystemMessage
    {
        IDomainProcess DomainProcess { get; }
        List<IComplexEventAction> ComplexEvents { get; }
        List<IViewModelInfo> ViewModelInfos { get; }
    }
}
