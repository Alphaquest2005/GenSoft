using System.Collections.Generic;
using SystemInterfaces;
using GenSoft.Interfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    public interface ILoadProcessComplexEvents : IProcessSystemMessage
    {
        List<IComplexEventAction> ComplexEvents { get; }
    }

    public interface ILoadDomainProcessViewModels : IProcessSystemMessage
    {
        List<IViewModelInfo> ViewModelInfos { get; }
    }
}
