using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using FluentValidation;
using GenSoft.Interfaces;
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
        IEntityKeyValuePair CurrentProperty { get; }
        ObservableDictionary<string, dynamic> ChangeTracking { get; }

        void NotifyPropertyChanged(string propertyName);

        ReactiveProperty<RowState> RowState { get; }

        ObservableList<IDynamicEntity> ParentEntities { get; }

        IViewAttributeDisplayProperties DisplayProperties { get; }

      

    }




    public interface IEntityListViewModel : IEntityViewModel
    {
        new ReactiveProperty<IProcessStateList> State { get; }
        
        
        ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; }
        

    }


}
