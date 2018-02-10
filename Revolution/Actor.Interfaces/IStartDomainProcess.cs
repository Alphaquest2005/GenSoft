using System.Collections.Generic;
using SystemInterfaces;
using ViewModel.Interfaces;

namespace Actor.Interfaces
{
    public interface ILoadProcessComplexEvents : IProcessSystemMessage
    {
        IReadOnlyList<IComplexEventAction> ComplexEvents { get; }
    }

    public interface ILoadDomainProcessViewModels : IProcessSystemMessage
    {
        List<IViewModelInfo> ViewModelInfos { get; }
    }
    public interface ILoadDataService : IProcessSystemMessage
    {
        string EntityTypeName { get; }
    }
}
