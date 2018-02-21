using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Actor.Interfaces;
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

        public static BootStrapper Instance { get; }
        public static CompositionContainer Container { get;  }

        public void StartUp(bool autoContinue, List<IComplexEventAction> processComplexEvents, List<IViewModelInfo> processViewModelInfos)
        {
            try
            {


                var x = Container.GetExport<IActorBackBone>().Value;
                x.Initialize(autoContinue, processComplexEvents, processViewModelInfos);

            }
            catch (Exception)
            {

                throw;
            }
        }
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
            //Todo: add retry logic
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
