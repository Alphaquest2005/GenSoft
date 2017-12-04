using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using SystemInterfaces;
using Common.DataEntites;
using FluentValidation.Results;
using GenSoft.Interfaces;
using JB.Collections.Reactive;
using Reactive.Bindings;
using RevolutionEntities.Process;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using ISystemProcess = SystemInterfaces.ISystemProcess;


namespace Core.Common.UI
{



    
    public partial class EntityListViewModel : BaseViewModel<EntityListViewModel>, IEntityListViewModel
    {

        
        protected ValidationResult ValidationResults = new ValidationResult();
        protected static EntityListViewModel _instance = null;
        public static EntityListViewModel Instance => _instance;
        
        public EntityListViewModel(){}

        public EntityListViewModel(IEntityViewInfo viewInfo, List<IViewModelEventSubscription<IViewModel, IEvent>> eventSubscriptions, List<IViewModelEventPublication<IViewModel, IEvent>> eventPublications, List<IViewModelEventCommand<IViewModel, IEvent>> commandInfo, ISystemProcess process, Type orientation, int priority, IViewAttributeDisplayProperties displayProperties) : base(process,viewInfo,eventSubscriptions,eventPublications,commandInfo, orientation, priority)
        {
            DisplayProperties = displayProperties;
            ViewInfo = viewInfo;
            State.Subscribe(x => UpdateLocalState(x));
            CurrentEntity.Subscribe(x => ChangeTracking.Clear());
            CurrentProperty.Subscribe(x => OnValueChanged(x));
            _instance = this;
        }

        
        private void UpdateLocalState(IProcessStateList state)
        {
            if (state == null) return;
            //CurrentEntity.Value = state.Entity;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (EntitySet.Value.SequenceEqual(state.EntitySet)) return;
                EntitySet.Value = new ObservableList<IDynamicEntity>(state.EntitySet.ToList());
                EntitySet.Value.Reset();

                if (!SelectedEntities.Value.SequenceEqual(state.SelectedEntities.ToList()))
                    SelectedEntities.Value = new ObservableList<IDynamicEntity>(state.SelectedEntities.ToList());
            }));
        }


        public ReactiveProperty<IEntityKeyValuePair> CurrentProperty { get; } = new ReactiveProperty<IEntityKeyValuePair>();
      



        private void OnValueChanged(object entityKeyValuePair)
        {
            if (RowState.Value == SystemInterfaces.RowState.Modified)
                ChangeTracking.AddOrUpdate(CurrentProperty.Value.Key, CurrentProperty.Value.Value);
        }


        //ReactiveProperty<IProcessStateList> IEntityViewModel.State
        //{
        //    get { return new ReactiveProperty<IProcessStateList>(_state.Value); }
        //}

        //HACK:NEVER USER THIS IMPLEMENTATION - FOR SOME FUCKED UP REASON IT NOT RAISING CHANGE NOTIFICATIONS EVEN IF YOU NEVER CALL THE SETTER....
        // public ReactiveProperty<IProcessStateList> State => new ReactiveProperty<IProcessStateList>();

        
        public new IEntityViewInfo ViewInfo { get; }

        public ReactiveProperty<IProcessStateList> State { get; } = new ReactiveProperty<IProcessStateList>();

        

        ReactiveProperty<IProcessStateEntity> IEntityViewModel.State { get; } = new ReactiveProperty<IProcessStateEntity>();

        public ReactiveProperty<IDynamicEntity> CurrentEntity { get; } = new ReactiveProperty<IDynamicEntity>();
        
        public virtual ReactiveProperty<ObservableList<IDynamicEntity>> EntitySet { get; } = new ReactiveProperty<ObservableList<IDynamicEntity>>(new ObservableList<IDynamicEntity>());
        

        public ReactiveProperty<ObservableList<IDynamicEntity>> SelectedEntities { get; } = new ReactiveProperty<ObservableList<IDynamicEntity>>(new ObservableList<IDynamicEntity>());


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
            this.OnPropertyChanged(property);
        }


       

        public ObservableDictionary<string, dynamic> ChangeTracking { get; } = new ObservableDictionary<string, dynamic>();
        public void NotifyPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);
        }

        public ObservableList<IDynamicEntity> ParentEntities { get; } = new ObservableBindingList<IDynamicEntity>();
        public IViewAttributeDisplayProperties DisplayProperties { get; }
        public IDomainProcess DomainProcess { get; }

        private ObservableBindingList<IDynamicEntity> _changeTrackingList = new ObservableBindingList<IDynamicEntity>();
        

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