using System;
using System.Linq;
using System.Reactive.Linq;
using SystemInterfaces;
using Common;
using EventAggregator;
using Process.WorkFlow;
using RevolutionEntities.Process;

namespace AsycudaXMLImport
{
    public class Import
    {
        public static ISystemSource Source => new Source(Guid.NewGuid(), $"Addin:<AsycudaXMLImport>", new SourceType(typeof(Import)), Processes.IntialSystemProcess, Processes.IntialSystemProcess.MachineInfo);
        static Import()
        {
            EventMessageBus.Current.GetEvent<IStartAddin>(
                new StateCommandInfo(Processes.IntialSystemProcess,
                    RevolutionData.Context.CommandFunctions.UpdateCommandData("AsycudaXMLImport",
                        RevolutionData.Context.Addin.Commands.StartAddin), Guid.NewGuid()), Source).Where(x => x.Action.Action == "Import" && x.Action.Addin == "AsycudaXMLImport").Subscribe(x => OnImport(x.Entity));
        }

        private static void OnImport(IDynamicEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
