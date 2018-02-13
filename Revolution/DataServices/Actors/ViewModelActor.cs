using System;
using System.Diagnostics;
using System.Linq;
using SystemInterfaces;

using BootStrapper;
using EventAggregator;
using EventMessages.Events;
using RevolutionData.Context;
using RevolutionEntities.Process;
using ViewMessages;
using ViewModel.Interfaces;
using ViewModelInterfaces;

namespace DataServices.Actors
{
    public class ViewModelActor : BaseActor<ViewModelActor>, IViewModelService
    {
        

        public ViewModelActor(ILoadViewModel msg,ISystemProcess process) : base(msg.ViewModelInfo.Key,process)
        {
            HandleProcessViews(msg);
        }
        

        private void HandleProcessViews(ILoadViewModel pe)
        {
            this.GetType()
                .GetMethod("LoadEntityViewModel")
                .MakeGenericMethod(pe.ViewModelInfo.ViewModelType)
                .Invoke(this, new object[] {pe});
            
        }

        public void LoadEntityViewModel<TViewModel>(LoadViewModel vmInfo) where TViewModel : IViewModel
        {
            try
            {
                var process = vmInfo.Process;
                var viewInfo = vmInfo.ViewModelInfo;
                var vm = CreateViewModel<TViewModel>(process, viewInfo);

                if (vm is IEntityViewModel)
                {
                    // this is not the same event THIS IS IVIEWMODEL
                EventMessageBus.Current.Publish(
                    new ViewModelCreated<IViewModel>(vm,
                        new StateEventInfo(vmInfo.Process, RevolutionData.Context.EventFunctions.UpdateEventData(vm.ViewInfo.Name, RevolutionData.Context.ViewModel.Events.ViewModelCreated)),
                        vmInfo.Process, Source));
                }
                else
                {
                    // this is not the same event THIS IS TVIEWMODEL
                    EventMessageBus.Current.Publish(
                        new ViewModelCreated<TViewModel>(vm,
                            new StateEventInfo(vmInfo.Process, RevolutionData.Context.EventFunctions.UpdateEventData(vm.ViewInfo.Name, RevolutionData.Context.ViewModel.Events.ViewModelCreated)),
                            vmInfo.Process, Source));
                }
               

                
            }
            catch (Exception ex)
            {
                Debugger.Break();
                EventMessageBus.Current.Publish(new ProcessEventFailure(failedEventType: typeof(ILoadViewModel),
                        failedEventMessage: vmInfo,
                        expectedEventType: typeof(IViewModelCreated<IDynamicViewModel<TViewModel>>),
                        exception: ex,
                        source: Source,
                        processInfo: new StateEventInfo(vmInfo.Process,
                           EventFunctions.UpdateEventData(ex.Message, RevolutionData.Context.Process.Events.Error))));
            }

        }



        public TViewModel CreateViewModel<TViewModel>(ISystemProcess vmInfoProcess, IViewModelInfo vmInfo)
            where TViewModel : IViewModel
        {
            try
            {


                var concreteVM = BootStrapper.BootStrapper.Container.GetExportedTypes<TViewModel>().FirstOrDefault() ??
                                 BootStrapper.BootStrapper.Container.GetExportedType(vmInfo.ViewModelType);


                object[] objects;
                if (vmInfo.DisplayProperties.ReadProperties.Properties.Any())
                {
                    objects = new object[]
                    {
                        vmInfoProcess, vmInfo.ViewInfo, vmInfo.Subscriptions,
                        vmInfo.Publications, vmInfo.Commands, vmInfo.Orientation,
                        vmInfo.Priority, vmInfo.DisplayProperties
                    };
                }
                else
                {
                    objects = new object[]
                    {
                        vmInfoProcess, vmInfo.ViewInfo, vmInfo.Subscriptions,
                        vmInfo.Publications, vmInfo.Commands, vmInfo.Orientation,
                        vmInfo.Priority
                    };
                }


                var vm = (TViewModel) Activator.CreateInstance(concreteVM,
                    objects);
                vm.Visibility.Value = vmInfo.Visibility;
                foreach (var v in vmInfo.ViewModelInfos.Where(x => x != null))
                {
                    var cvm = (IViewModel) typeof(ViewModelActor).GetMethod("CreateViewModel")
                        .MakeGenericMethod(v.ViewModelType)
                        .Invoke(this, new object[] {vmInfoProcess, v});
                    cvm.Visibility.Value = v.Visibility;
                    vm.ViewModels.Add(cvm);
                }
                return vm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


}