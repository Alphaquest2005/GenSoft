using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using SystemInterfaces;
using Actor.Interfaces;
using MefContrib.Hosting.Generics;
using Utilities;
using ViewModel.Interfaces;


namespace BootStrapper
{
    public class BootStrapper
    {
        static BootStrapper()
        {
            try
            {
                Instance = new BootStrapper();

                var catalog =
                    new DirectoryCatalog(
                        Path.GetDirectoryName(
                            Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path)),
                        "*.dll");
                var genericCatalog = new System.ComponentModel.Composition.Hosting.Extension.GenericCatalog(catalog);
                Container = new CompositionContainer(genericCatalog);
                
            }
            catch (Exception)
            {

                throw;
            }



        }

        public void StartUp(bool autoContinue, Assembly dbContextAssembly, Assembly entitiesAssembly, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewModelInfos)
        {
            try
            {
                var x = Container.GetExport<IActorBackBone>().Value;
                x.Intialize(autoContinue,  viewModelInfos);
   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static BootStrapper Instance { get; }


        public void StartUp(bool autoRun, List<IMachineInfo> machineInfo, List<IProcessInfo> processInfos, List<IComplexEventAction> complexEventActions, List<IViewModelInfo> viewModelInfos)
        {
            try
            {
                var x = Container.GetExport<IActorBackBone>().Value;
                x.Intialize(autoRun,machineInfo, processInfos,complexEventActions, viewModelInfos);
               
            }
            catch (Exception)
            {

                throw;
            }



            // var t = Container.GetExport<ISummaryViewModel<IPatientInfo>>().Value;
            // EventMessageBus.Current.Publish(Container.GetExport<ICreateSummaryViewModel<IPatientInfo>>().Value);
            //EventMessageBus.Current.Publish(Container.GetExport<IStartDataService>().Value, new MessageSource(this.ToString()));

        }

        public static CompositionContainer Container { get;  }
    }

    public static class CompositionContainerExtensions
    {
        public static dynamic GetConcreteInstance<T>(this CompositionContainer container, Type type)
        {
            var itmType = BootStrapper.Container.GetExportedTypes<T>().FirstOrDefault() ?? BootStrapper.Container.GetExportedType(type);
            return Activator.CreateInstance(itmType);
        }
        public static dynamic GetConcreteInstance(this CompositionContainer container, Type type)
        {
            var itmType = BootStrapper.Container.GetExportedTypes(type).FirstOrDefault() ?? BootStrapper.Container.GetExportedType(type);
            return Activator.CreateInstance(itmType);
        }

        public static dynamic GetConcreteType<T>(this CompositionContainer container, Type type)
        {
            return BootStrapper.Container.GetExportedTypes<T>().FirstOrDefault() ?? BootStrapper.Container.GetExportedType(type);
        }
        public static dynamic GetConcreteType(this CompositionContainer container, Type type)
        {
            return BootStrapper.Container.GetExportedTypes(type).FirstOrDefault() ?? BootStrapper.Container.GetExportedType(type);
        }
        public static dynamic GetExportedType(this CompositionContainer container, Type type)
        {
            try
            {
                if (type.IsGenericType)
                {
                    var t = 
                        container.GetType()
                            .GetMethod("GetExportedValueOrDefault", new Type[] {})
                            .MakeGenericMethod(type.GetGenericTypeDefinition()
                                .MakeGenericType(type.GenericTypeArguments))
                            .Invoke(container, new object[] {})?
                            .GetType();
                    if (t != null) return t;
                    t = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .FirstOrDefault(type.IsAssignableFrom);
                    return t;
                }
                     ;

                return container.GetType().GetMethod("GetExportedValueOrDefault", new Type[] { }).MakeGenericMethod(type).Invoke(container, new object[] { })?.GetType();
            }
            catch (Exception)
            {
                
                throw;
            }
            


        }
    }
}
