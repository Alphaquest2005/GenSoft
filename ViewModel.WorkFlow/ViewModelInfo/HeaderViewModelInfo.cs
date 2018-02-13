using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Common;
using DomainUtilities;
using EventMessages.Commands;
using JB.Collections.Reactive;
using Process.WorkFlow;
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
        public static ViewModelInfo HeaderViewModel()
        {
            return new ViewModelInfo
            (
                Processes.IntialSystemProcess,
                new ViewInfo("HeaderViewModel", "", ""),
                new List<IViewModelEventSubscription<IViewModel, IEvent>>
                {
                    new ViewEventSubscription<IHeaderViewModel, IViewModelInitialized>(
                        $"HeaderViewModel-IViewModelInitialized",
                        Processes.IntialSystemProcess,
                        e => e != null && e.ViewModel.ViewInfo.Name == "HeaderViewModel",
                        new List<Func<IHeaderViewModel, IViewModelInitialized, bool>>(),
                        (v,e) =>
                        {
                            var entityType = DynamicEntityTypeExtensions.GetOrAddDynamicEntityType("Application");
                            EventAggregator.EventMessageBus.Current.Publish( new LoadEntitySet(entityType,
                                new StateCommandInfo(v.Process,
                                    Context.CommandFunctions.UpdateCommandData("Application",Context.Entity.Commands.LoadEntitySetWithChanges)),
                                v.Process, v.Source), v.Source);
                        }, new StateEventInfo(Processes.IntialSystemProcess, Context.EventFunctions.UpdateEventData("HeaderViewModel" ,RevolutionData.Context.ViewModel.Events.Initialized), Guid.NewGuid())),

                    new ViewEventSubscription<IHeaderViewModel, IEntitySetLoaded>(
                        $"HeaderViewModel-IUpdateProcessStateList",
                        Processes.IntialSystemProcess,
                        e => e != null  && e.EntityType.Name == "Application",
                        new List<Func<IHeaderViewModel, IEntitySetLoaded, bool>>(),
                        (v,e) =>
                        {
                            if (v.CurrentEntity.Value != null) return;
                            if (e.EntitySet != null && v.Entities.Value != null &&
                                v.Entities.Value.SequenceEqual(e.EntitySet)) return;
                            v.Entities.Value = new ObservableList<IDynamicEntity>(e.EntitySet.ToList());
                        }, new StateEventInfo(Processes.IntialSystemProcess, Context.EventFunctions.UpdateEventData("Application" ,RevolutionData.Context.Entity.Events.EntitySetLoaded), Guid.NewGuid())),

                },
                new List<IViewModelEventPublication<IViewModel, IEvent>>
                {
                    new ViewEventPublication<IHeaderViewModel, IViewModelInitialized>(
                        key:$"HeaderViewModel-IViewModelInitialized",
                        subject:v => v.ViewModelState,
                        subjectPredicate:new List<Func<IHeaderViewModel, bool>>{ v => v.ViewModelState.Value == ViewModelState.Initialized},
                        messageData:v => new ViewEventPublicationParameter(new object[] {v},new RevolutionEntities.Process.StateEventInfo(v.Process, Context.EventFunctions.UpdateEventData("HeaderViewModel", Context.ViewModel.Events.Initialized)),v.Process,v.Source)),

                    new ViewEventPublication<IHeaderViewModel, ICurrentApplicationChanged>(
                        key:$"HeaderViewModel-CurrentApplicationChanged",
                        subject:v =>  (IObservable<dynamic>)v.CurrentEntity,//.WhenAnyValue(x => x.Value),
                        subjectPredicate:new List<Func<IHeaderViewModel, bool>>{},
                        messageData:s => new ViewEventPublicationParameter(new object[] {s.CurrentEntity.Value},new RevolutionEntities.Process.StateEventInfo(s.Process, Context.EventFunctions.UpdateEventData(s.Process.Name, Context.Process.Events.CurrentApplicationChanged)),s.Process,s.Source)),
                },
                new List<IViewModelEventCommand<IViewModel, IEvent>> {},
                typeof(IHeaderViewModel),
                typeof(IHeaderViewModel), 0,
                new ViewAttributeDisplayProperties(
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>()),
                    new AttributeDisplayProperties(new Dictionary<string, Dictionary<string, string>>())
                ),
                new List<IViewModelInfo>());
        }
    }
}