﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using SystemInterfaces;
using ReactiveUI;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
    public class SigninViewModelInfo
    {
        public static ViewModelInfo SigninViewModel = new ViewModelInfo
            (
            2,
            new ViewInfo("", "", ""),
            new List<IViewModelEventSubscription<IViewModel, IEvent>>
            {
                new ViewEventSubscription<ISigninViewModel, IProcessStateMessage>(
                    2, e => e != null && e.State.Entity.EntityType == "ISignInInfo",
                    new List<Func<ISigninViewModel, IProcessStateMessage, bool>>(),
                    (v, e) =>
                    {
                        if(v.State.Value == e.State) return;
                        v.State.Value = e.State;
                    }
                    )

            }, new List<IViewModelEventPublication<IViewModel, IEvent>>
            {
                   


                new ViewEventPublication<ISigninViewModel, IViewStateLoaded<ISigninViewModel,IProcessState>>(
                    key:"ILoginModelViewStateLoaded", 
                    subject:v => v.State,
                    subjectPredicate:new List<Func<ISigninViewModel, bool>>
                    {
                        v => v.State != null 
                    },
                    messageData:s => new ViewEventPublicationParameter(new object[] {s,s.State.Value},new StateEventInfo(s.Process.Id, Context.View.Events.ProcessStateLoaded),s.Process,s.Source))
            }, new List<IViewModelEventCommand<IViewModel,IEvent>>
            {
                   
                new ViewEventCommand<ISigninViewModel, IGetEntityWithChanges>(
                    key:"UserName",
                    subject:v => v.ChangeTracking.DictionaryChanges,
                    commandPredicate: new List<Func<ISigninViewModel, bool>>
                    {
                        v => v.ChangeTracking.Keys.Contains("Usersignin") && v.ChangeTracking.Keys.Count == 1
                    },
                    messageData: s => new ViewEventCommandParameter(new object[] {s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)},new StateCommandInfo(s.Process.Id, Context.EntityView.Commands.GetEntityView),s.Process,s.Source)),

                new ViewEventCommand<ISigninViewModel, IGetEntityWithChanges>(
                    key:"ValidateUserInfo",
                    commandPredicate:new List<Func<ISigninViewModel, bool>>
                    {
                        v => v.ChangeTracking.Keys.Contains("Usersignin")
                                                    
                    },
                    subject:s => Observable.Empty<ReactiveCommand<IViewModel, Unit>>(),
                               
                    messageData:s => new ViewEventCommandParameter(new object[] {s.ChangeTracking.ToDictionary(x => x.Key, x => x.Value)},new StateCommandInfo(s.Process.Id, Context.EntityView.Commands.GetEntityView),s.Process,s.Source)),

                    

            }, typeof(ISigninViewModel), typeof(IBodyViewModel), 0);
    }
}