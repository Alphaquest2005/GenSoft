using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using JB.Collections.Reactive;


namespace ViewModel.Interfaces
{
    
    public interface IScreenLayoutViewModel: IViewModel
    {
        IScreenModel Instance { get; }
        ObservableList<IViewModel> HeaderViewModels { get; }
        ObservableList<IViewModel> LeftViewModels { get; }
        ObservableList<IViewModel> RightViewModels { get; }
        ObservableList<IViewModel> FooterViewModels { get; }
        ObservableList<IViewModel> BodyViewModels { get;}
        Dictionary<string,IViewModel> CacheViewModels { get; }
    }
}