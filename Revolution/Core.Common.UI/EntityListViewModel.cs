using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using SystemInterfaces;
using Common;
using FluentValidation;
using FluentValidation.Results;
using JB.Collections.Reactive;
using Reactive.Bindings;
using ReactiveUI;
using RevolutionEntities;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using ViewModelInterfaces;


namespace Core.Common.UI
{



    
    public partial class EntityListViewModel : BaseViewModel<EntityListViewModel>, IEntityListViewModel
    {

        
        protected ValidationResult ValidationResults = new ValidationResult();
        protected static EntityListViewModel _instance = null;
        public static EntityListViewModel Instance => _instance;
        
        public EntityListViewModel() { }

        public EntityListViewModel(IViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation, int priority) : base(process,viewInfo,eventSubscriptions,eventPublications,commandInfo, orientation, priority)
        {
            
            State.WhenAnyValue(x => x.Value).Subscribe(x => UpdateLocalState(x));
            CurrentEntity.WhenAnyValue(x => x.Value).Subscribe(x => ChangeTracking.Clear());

            _instance = this;
        }

        private void UpdateLocalState(IProcessStateList state)
        {
            if (state == null) return;
            //CurrentEntity.Value = state.Entity;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                EntitySet.Value = new ObservableList<IDynamicEntity>(state.EntitySet.ToList());
                EntitySet.Value.Reset();

                if (!SelectedEntities.Value.SequenceEqual(state.SelectedEntities.ToList()))
                    SelectedEntities.Value = new ObservableList<IDynamicEntity>(state.SelectedEntities.ToList());
            }));
        }


        private IEntityKeyValuePair _currentProperty;

        public IEntityKeyValuePair CurrentProperty
        {
            get { return _currentProperty; }
            set
            {
                _currentProperty = value;
                _currentProperty?.WhenAnyValue(x => x.Value).Subscribe(x => OnValueChanged(x));
            }
        }



        private void OnValueChanged(object entityKeyValuePair)
        {
            ChangeTracking.AddOrUpdate(_currentProperty.Key, _currentProperty.Value);
        }


        //ReactiveProperty<IProcessStateList> IEntityViewModel.State
        //{
        //    get { return new ReactiveProperty<IProcessStateList>(_state.Value); }
        //}

        //HACK:NEVER USER THIS IMPLEMENTATION - FOR SOME FUCKED UP REASON IT NOT RAISING CHANGE NOTIFICATIONS EVEN IF YOU NEVER CALL THE SETTER....
        // public ReactiveProperty<IProcessStateList> State => new ReactiveProperty<IProcessStateList>();

        private ReactiveProperty<IProcessStateList> _state = new ReactiveProperty<IProcessStateList>();
        public ReactiveProperty<IProcessStateList> State
        {
            get { return _state; }
            set { this.RaiseAndSetIfChanged(ref _state, value);}
        }

        

        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State => new ReactiveProperty<IProcessStateEntity>(new ProcessStateEntity(State.Value.Process, CurrentEntity.Value, State.Value.StateInfo.ToStateInfo()));

        private ReactiveProperty<IDynamicEntity> _currentEntity = new ReactiveProperty<IDynamicEntity>(null, ReactivePropertyMode.DistinctUntilChanged);
        public ReactiveProperty<IDynamicEntity> CurrentEntity
        {
            get { return _currentEntity; }
            set { this.RaiseAndSetIfChanged(ref _currentEntity, value); }
        }


        public virtual ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet
        {
            get { return _entitySet; }
            set { this.RaiseAndSetIfChanged(ref _entitySet, value); }
        }


        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities => new ReactiveProperty<ObservableList<IDynamicEntity>>(new ObservableList<IDynamicEntity>());


        public dynamic GetValue([CallerMemberName] string property = "UnspecifiedProperty")
        {
            if (CurrentEntity.Value == null) return null;
            var prop =  CurrentEntity.Value.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if(prop == null) return null;
            return ChangeTracking.ContainsKey(property)
                ? ChangeTracking[property]
                : prop.GetValue(CurrentEntity.Value);
        }
        protected dynamic GetOriginalValue([CallerMemberName] string property = "UnspecifiedProperty")
        {
            var propertyInfo = CurrentEntity.Value.GetType().GetProperty(property);
            if (propertyInfo != null)
                return propertyInfo.GetValue(CurrentEntity);
            return null;
        }
        protected bool GetPropertyIsChanged([CallerMemberName] string property = "UnspecifiedProperty")
        {
            return ChangeTracking.ContainsKey(property);
        }
        public void SetValue(dynamic value, [CallerMemberName] string property = "UnspecifiedProperty")
        {
            if (CurrentEntity.Value.GetType().GetProperty(property, BindingFlags.Public |BindingFlags.SetProperty| BindingFlags.Instance) == null) return;
            if (!ChangeTracking.ContainsKey(property))
            {
                //HACK: doing this shit cuz jbcollection generating some error long after add no control to prevent error
                try
                {
                    ChangeTracking.AddOrUpdate(property, value);
                }
                catch
                {
                }

            }
            else
            {
                ChangeTracking[property] = value;
            }
            this.RaisePropertyChanged(property);
        }


       

        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();
        public void NotifyPropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        private ObservableBindingList<IDynamicEntity> _changeTrackingList = new ObservableBindingList<IDynamicEntity>();
        private ReactiveProperty<ObservableList<IDynamicEntity>> _entitySet = new ReactiveProperty<ObservableList<IDynamicEntity>>(new ObservableList<IDynamicEntity>());


        public ObservableBindingList<IDynamicEntity> ChangeTrackingList
        {
            get { return _changeTrackingList; }
            set { _changeTrackingList = value; }
        }


        public IEnumerable GetErrors(string propertyName)
        {
            return ValidationResults.Errors.Where(x => x.PropertyName == propertyName);
        }

        public bool HasErrors => !ValidationResults.IsValid;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;




    }

}