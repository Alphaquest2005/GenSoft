using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SystemInterfaces;
using Asycuda;
using Asycuda421;
using Common;
using EventAggregator;
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
                        RevolutionData.Context.Addin.Commands.StartAddin), Guid.NewGuid()), Source).Where(x => x.Action.Action == "Import" && x.Action.Addin == "AsycudaXMLImport").Subscribe(x => OnImport(x.Entity));
        }

        private static void OnImport(IDynamicEntity docSet)
        {
            //StatusModel.Timer("Importing Documents");
            //import asycuda xml id and details
            if (docSet.Id == 0)
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
                        AsycudaToDataBase421.Instance.Import(a, docSet);
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
