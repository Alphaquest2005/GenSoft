using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Actor.Interfaces;
using Common;
using DomainUtilities;
using EventMessages.Commands;
using JB.Collections.Reactive;
using Reactive.Bindings;
using Reactive.EventAggregator;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;

namespace RevolutionData
{
}

namespace RevolutionData
{
    public class HeaderViewModelInfo
    {
        public static ViewModelInfo HeaderViewModel(int processId)
        {
            return new ViewModelInfo
            (
                processId,
                new ViewInfo("HeaderViewModel", "", ""),
                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IHeaderViewModel, IViewModelIntialized>(
                        $"HeaderViewModel-IViewModelIntialized",
                        processId,
                        e => e != null && e.ViewModel.ViewInfo.Name == "HeaderViewModel",
                        new List<Func<IHeaderViewModel, IViewModelIntialized, bool>>(),
                        (v,e) =>
                        {
                            var entityType = DynamicEntityTypeExtensions.AddDynamicEntityTypes("Application");
                           EventAggregator.EventMessageBus.Current.Publish( new LoadEntitySet(entityType,
                                new StateCommandInfo(v.Process.Id,
                                    Context.Entity.Commands.LoadEntitySetWithChanges),
                                v.Process, v.Source), v.Source);
                        }),

                    new ViewEventSubscription<IHeaderViewModel, IEntitySetLoaded>(
                        $"HeaderViewModel-IUpdateProcessStateList",
                        processId,
                        e => e != null  && e.EntityType.Name == "Application",
                        new List<Func<IHeaderViewModel, IEntitySetLoaded, bool>>(),
                        (v,e) =>
                        {
                            if (e.EntitySet != null && v.Entities.Value != null && v.Entities.Value.SequenceEqual(e.EntitySet)) return;
                            v.Entities.Value = new ObservableList<IDynamicEntity>(e.EntitySet.ToList());
                        }),

                    //new ViewEventSubscription<IHeaderViewModel, ICleanUpSystemProcess>(
                    //    "Footer-ICleanUpSystemProcess",
                    //    processId,
                    //    e => e != null,
                    //    new List<Func<IHeaderViewModel, ICleanUpSystemProcess, bool>> { },
                    //    (s, e) =>
                    //    {
                    //        if (Application.Current == null)
                    //        {
                    //            s.Entities.Value.Clear();
                    //            s.Entities.Value.Reset();
                    //        }
                    //        else
                    //        {
                    //            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //            {
                    //                s.Entities.Value.Clear();
                    //                s.Entities.Value.Reset();
                    //            }));
                    //        }
                    //    }),
                },
                new List<IViewModelEventPublication<IViewModel, IEvent>>
                {
                    new ViewEventPublication<IHeaderViewModel, IViewModelIntialized>(
                        key:$"HeaderViewModel-IViewModelIntialized",
                        subject:v => v.ViewModelState,
                        subjectPredicate:new List<Func<IHeaderViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Intialized},
                        messageData:v => new ViewEventPublicationParameter(new object[] {v},new RevolutionEntities.Process.StateEventInfo(v.Process.Id, Context.View.Events.Intitalized),v.Process,v.Source)),

                    new ViewEventPublication<IHeaderViewModel, ICurrentEntityChanged>(
                        key:$"HeaderViewModel-CurrentEntityChanged",
                        subject:v =>  (IObservable<dynamic>)v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                        subjectPredicate:new List<Func<IHeaderViewModel, bool>>{},
                        messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value},new RevolutionEntities.Process.StateEventInfo(s.Process.Id, Context.View.Events.CurrentEntityChanged),s.Process,s.Source)),
                },
                new List<IViewModelEventCommand<IViewModel, IEvent>> {},
                typeof(IHeaderViewModel),
                typeof(IHeaderViewModel), 0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ));
        }
    }
}