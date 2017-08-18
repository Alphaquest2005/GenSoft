﻿using System.Collections.Generic;
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
        ObservableDictionary<string, dynamic> ChangeTracking { get; }

        void NotifyPropertyChanged(string propertyName);

    }




    public interface IEntityListViewModel : IEntityViewModel
    {
        IEntityListViewModel Instance { get; }
        new ReactiveProperty<IProcessStateList> State { get; }
        ReactiveProperty<IDynamicEntity> CurrentEntity { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; }
        ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; }
        
    }


}
