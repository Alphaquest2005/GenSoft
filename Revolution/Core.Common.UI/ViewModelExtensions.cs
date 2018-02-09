﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SystemInterfaces;
using BootStrapper;
using CommonMessages;
using EventAggregator;
using EventMessages.Events;
using Reactive.Bindings;

using RevolutionEntities.Process;
using ViewModel.Interfaces;


namespace Core.Common.UI
{
    public static class ViewModelExtensions
    {
        public static void WireEvents(this IViewModel viewModel)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            viewModel.ViewModelState.Value = ViewModelState.NotIntialized;


            foreach (var itm in viewModel.CommandInfo)
            {
                var subject = itm.Subject.Invoke(viewModel);


                if (subject.GetType() == Observable.Empty<ReactiveCommand<IViewModel>>().GetType())
                {
                    var publishMessage = CreateCommandMessageAction<IViewModel>(viewModel, itm);
                    var cmd = new ReactiveCommand<IViewModel>();
                    cmd.Subscribe(publishMessage);
                    viewModel.Commands.Add(itm.Key, cmd);
                }
                else
                {
                    var publishMessage = CreateCommandMessageAction<dynamic>(viewModel, itm);
                    subject.Where(x => itm.CommandPredicate.All(z => z.Invoke(viewModel)))
                        .Subscribe<dynamic>(publishMessage);
                }
            }




            var subscriptions =
                new ConcurrentBag<IViewModelEventSubscription<IViewModel, IEvent>>(viewModel.EventSubscriptions);

            Parallel.ForEach(subscriptions,
                new ParallelOptions() {MaxDegreeOfParallelism = Environment.ProcessorCount}, (itm) =>
                {
                    typeof(ViewModelExtensions)
                        .GetMethod("Subscribe")
                        .MakeGenericMethod(itm.EventType, viewModel.GetType())
                        .Invoke(viewModel,
                            new object[] {viewModel, itm.EventPredicate, itm.ActionPredicate, itm.Action});
                });
            

           


            foreach (var itm in viewModel.EventPublications)
            {
                var subject = itm.Subject.Invoke(viewModel);

                var publishMessage = CreatePublishMessageAction(viewModel, itm);
                subject.Where(x => itm.SubjectPredicate.All(z => z.Invoke(viewModel)))
                    .Subscribe<dynamic>(publishMessage);
            }

            viewModel.ViewModelState.Value = ViewModelState.Intialized;
           



        }

        

        private static Action<dynamic> CreatePublishMessageAction(IViewModel viewModel, IViewModelEventPublication<IViewModel, IEvent> itm)
        {
            void PublishMessage(dynamic x)
            {
                var param = itm.MessageData.Invoke(viewModel);
                var paramArray = param.Params.ToList();
                paramArray.Add(param.ProcessInfo);
                paramArray.Add(param.Process);
                paramArray.Add(param.Source);
                var concreteEvent = BootStrapper.BootStrapper.Container.GetConcreteType(itm.EventType);
                dynamic msg;
                if (concreteEvent == null)
                {
                    msg = new ProcessEventFailure(itm.EventType, new FailedMessageData(itm, param.ProcessInfo, param.Process, param.Source), itm.EventType, new InvalidOperationException($"Type:{itm.EventType.Name} not found in MEF - consider adding export to type."), param.ProcessInfo, viewModel.Source);
                }
                else
                {
                    msg = Activator.CreateInstance(concreteEvent, paramArray.ToArray());
                }
                EventMessageBus.Current.Publish(msg, viewModel.Source);
            }

            return PublishMessage;
        }

        private static Action<TResult> CreateCommandMessageAction<TResult>(IViewModel viewModel, IViewModelEventCommand<IViewModel, IEvent> itm)
        {
            void PublishMessage(TResult x)
            {
                var param = itm.MessageData.Invoke(viewModel);
                var paramArray = param.Params.ToList();
                paramArray.Add(param.ProcessInfo);
                paramArray.Add(param.Process);
                paramArray.Add(param.Source);
                var concreteEvent = BootStrapper.BootStrapper.Container.GetConcreteType(itm.EventType);
                //TODO: Replace MEF with Good IOC container - can't do <,>
                ProcessSystemMessage msg;
                if (concreteEvent == null)
                {
                    msg = new ProcessEventFailure(itm.EventType, new FailedCommandData(itm, param.ProcessInfo, param.Process, param.Source), itm.EventType, new InvalidOperationException("Type not found in MEF - consider adding export to type."), new StateEventInfo(param.ProcessInfo.Process, "Error", "Error occured getting type", "", param.ProcessInfo.State), viewModel.Source);
                }
                else
                {
                    msg = (ProcessSystemMessage) Activator.CreateInstance(concreteEvent, paramArray.ToArray());
                }

                EventMessageBus.Current.Publish(msg, viewModel.Source);
            }

            return PublishMessage;
        }

        public static void Subscribe<TEvent, TViewModel>(TViewModel viewModel, Func<TEvent, bool> eventPredicate,
            IEnumerable<Func<TViewModel, TEvent, bool>> predicate, Action<TViewModel, TEvent> action) where TEvent : class, IProcessSystemMessage
        {
            EventMessageBus.Current.GetEvent<TEvent>(new StateEventInfo(((IViewModel)viewModel).Process, new StateEvent("View Model Subscribtion", "some shit", "more shit")), ((IViewModel)viewModel).Source).Where(eventPredicate).Where(x => predicate.All(z => z.Invoke(viewModel, x))).Subscribe(x => action.Invoke(viewModel, x));
        }


        public static void VerifyConstuctorVsParameterArray(Type t, params object[] p)
        {
            System.Diagnostics.Debug.WriteLine("<---- foo");
            foreach (System.Reflection.ConstructorInfo ci in t.GetConstructors().Where(x => x.GetParameters().Count() == p.Count()))
            {
                System.Diagnostics.Debug.WriteLine(t.FullName + ci.ToString());
                var cp = ci.GetParameters().ToList();
                for (int j = 0; j < cp.Count; j++)
                {
                    System.Diagnostics.Debug.WriteLine($"Val:{cp[j].ParameterType.FullName == p[j].GetType().FullName} Cparm:{cp[j].ParameterType.FullName} || Oparam:{p[j].GetType().FullName}");
                }
            }
            
            System.Diagnostics.Debug.WriteLine("foo ---->");
        }
    }
}
