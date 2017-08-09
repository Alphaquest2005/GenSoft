using System.ComponentModel.Composition;
using SystemInterfaces;
using JB.Collections.Reactive;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    
    public interface ICacheViewModel : IViewModel
    {

    }

    public interface ISummaryListViewModel : IEntityListViewModel
    {

    }

   

    public interface IEntityCacheViewModel :ICacheViewModel
    {

    }

    public interface IEntityListCacheViewModel : ICacheViewModel, IEntityListViewModel 
    {

    }



}

