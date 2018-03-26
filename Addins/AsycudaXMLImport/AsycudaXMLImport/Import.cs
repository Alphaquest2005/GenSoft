using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Asycuda;
using Asycuda421;
using Common;
using Common.DataEntites;
using EventAggregator;
using EventMessages.Commands;
using JB.Collections.Reactive;
using Microsoft.Win32;
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
                        RevolutionData.Context.Addin.Commands.StartAddin), Guid.NewGuid()), Source).Where(x => x.Action.Action == "Import" && x.Action.Addin == "AsycudaXMLImport").Subscribe(OnImport);
        }

        private static void OnImport(IStartAddin msg)
        {
            //StatusModel.Timer("Importing Documents");
            //import asycuda xml id and details
            if (msg.Entity.Id == 0)
            {
                MessageBox.Show("Please Select Asycuda Document Set");
                return;
            }


            var od = new OpenFileDialog();
            od.DefaultExt = ".xml";
            od.Filter = "Xml Documents (.xml)|*.xml";
            od.Multiselect = true;
            var result = od.ShowDialog();
            if (result == true)
            {
                
                //StatusModel.Timer(string.Format("Importing {0} files", od.FileNames.Count()));
                //StatusModel.StartStatusUpdate("Importing files", od.FileNames.Count());
                //StatusModel.RefreshNow();
                foreach (var f in od.FileNames)
                {
                    if (ASYCUDA.CanLoadFromFile(f))
                    {
                        var a = Asycuda421.ASYCUDA.LoadFromFile(f);
                       var res = AsycudaToDataBase421.Instance.Import(a, msg.Entity);
                        var de = new DynamicEntity(
                            new DynamicEntityType("xcuda_ASYCUDA", "xcuda_ASYCUDA",
                                new List<IEntityKeyValuePair>(), new Dictionary<string, List<dynamic>>(),
                                new ObservableDictionary<string, Dictionary<int, dynamic>>(),
                                new List<IDynamicRelationshipType>(), new List<IDynamicRelationshipType>(), DynamicEntityType.NullEntityType(),
                                new ObservableList<IAddinAction>()), 0,
                            new Dictionary<string, object>());

                        EventMessageBus.Current.Publish(new UpdateEntityWithChanges(de, res.Properties.ToDictionary(x => x.Key, x => x.Value),
                            new StateCommandInfo(msg.Process, RevolutionData.Context.CommandFunctions.UpdateCommandData(de.EntityType.Name,RevolutionData.Context.Entity.Commands.CreateEntity)), msg.Process, Source));
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Can not Load file '{0}'", f));
                    }

                }
                     

                
                //StatusModel.StopStatusUpdate();

            }
           // StatusModel.StopStatusUpdate();
            MessageBox.Show("Complete");
        }
    }
}
