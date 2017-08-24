using System.Collections.Generic;
using System.ComponentModel.Composition;
using SystemInterfaces;
using FluentValidation;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ViewModel.Interfaces;

namespace ViewModelInterfaces
{
    
    public interface IEntityViewModel: IViewModel 
    {
        ReactiveProperty<IProcessStateEntity> State { get; }

        IEntityKeyValuePair CurrentProperty { get; }
        ObservableDictionary<string, dynamic> ChangeTracking { get; }

        void NotifyPropertyChanged(string propertyName);

        ReactiveProperty<RowState> RowState { get; }

    }




    public interface IEntityListViewModel :  IViewModel
    {
        ObservableDictionary<string, dynamic> ChangeTracking { get; }
        ReactiveProperty<IProcessStateList> State { get; }
        ReactiveProperty<IDynamicEntity> CurrentEntity { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; }
        void NotifyPropertyChanged(string propertyName);

        ReactiveProperty<RowState> RowState { get; }

    }


}
