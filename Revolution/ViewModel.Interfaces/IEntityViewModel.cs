using SystemInterfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModel.Interfaces;

namespace ViewModelInterfaces
{
    
    public interface IEntityViewModel: IViewModel 
    {
        new IEntityViewInfo ViewInfo { get; }
        ReactiveProperty<IProcessStateEntity> State { get; }

        ReactiveProperty<IDynamicEntity> CurrentEntity { get; }
        ReactiveProperty<IEntityKeyValuePair> CurrentProperty { get; }
        ObservableDictionary<string, dynamic> ChangeTracking { get; }

        void NotifyPropertyChanged(string propertyName);

        ReactiveProperty<RowState> RowState { get; }

        ObservableList<IDynamicEntity> ParentEntities { get; }

        string SuggestedName { get; }

        IViewAttributeDisplayProperties DisplayProperties { get; }

      

    }




    public interface IEntityListViewModel : IEntityViewModel
    {
        new ReactiveProperty<IProcessStateList> State { get; }
        
        
        ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; }
        

    }


}
