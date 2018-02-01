﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-ViewModels.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using System;
using SystemInterfaces;
using Core.Common.UI;
using EventAggregator;
using Process.WorkFlow;
using Reactive.Bindings;
using RevolutionData;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace ViewModels
{
    
    public class MainWindowViewModel : BaseViewModel<MainWindowViewModel>, IMainWindowViewModel
    {
        private static readonly MainWindowViewModel instance;

        static MainWindowViewModel()
        {
            instance = new MainWindowViewModel();
        }

        public static MainWindowViewModel Instance
        {
            get { return instance; }
        }

        public MainWindowViewModel()
            : base(Processes.IntialSystemProcess,
                new ViewInfo("MainWindowViewModel", "", ""),
                MainWindowViewModelInfo.MainWindowViewModel.Subscriptions,
                MainWindowViewModelInfo.MainWindowViewModel.Publications,
                MainWindowViewModelInfo.MainWindowViewModel.Commands,
                MainWindowViewModelInfo.MainWindowViewModel.Orientation,
                MainWindowViewModelInfo.MainWindowViewModel.Priority)
        {
            this.WireEvents();

            EventMessageBus.Current.GetEvent<ICurrentApplicationChanged>(Source).Subscribe(OnCurrentApplicationChanged);
        }

        private void OnCurrentApplicationChanged(ICurrentApplicationChanged currentEntityChanged)
        {
            if (currentEntityChanged.Entity == null) return;
            if (CurrentApplication?.Id == currentEntityChanged.Entity.Id) return;
            CurrentApplication = currentEntityChanged.Entity;
        }

        private IDynamicEntity _currentApplication;

        public IDynamicEntity CurrentApplication
        {
            get { return _currentApplication; }
            set
            {
                if (Equals(value, _currentApplication)) return;
                _currentApplication = value;
                OnPropertyChanged();
            }
        }


        public ReactiveProperty<IScreenModel> ScreenModel { get; } = new ReactiveProperty<IScreenModel>(); 

        


    }
}
