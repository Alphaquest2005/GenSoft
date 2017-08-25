using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using SystemInterfaces;
using Core.Common.UI;
using GenSoft.Entities;
using GenSoft.Interfaces;
using RevolutionEntities.Process;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using IEvent = SystemInterfaces.IEvent;

namespace ViewModel.WorkFlow
{
    public static class ViewModelInfoExtensions
    {
        public static IViewModelEventCommand<IViewModel, IEvent> CreateCustomCommand<TViewModel>(string cmdName, ViewModelCommands cmd) where TViewModel : IEntityViewModel
        {
            var cmdPredicates = new List<Func<TViewModel, bool>>();
            if (cmd.ExistingEntities) cmdPredicates.Add(v => v.CurrentEntity.Value.Id != 0);
            else cmdPredicates.Add(v => v.CurrentEntity.Value.Id <= 0);

            if (cmd.RequireAllFields) cmdPredicates.Add(v => v.ChangeTracking.Count == v.CurrentEntity.Value.PropertyList.Count(x => x.Key != nameof(IDynamicEntity.Id)));
            else cmdPredicates.Add(v => v.ChangeTracking.Any(z => v.CurrentEntity.Value.PropertyList.FirstOrDefault(x => x.Key == z.Key)?.Value != z.Value));

            var commandType = typeof(SystemInterfaces.IEntity).Assembly.GetType($"SystemInterfaces.{cmd.CommandTypes.Name}");
            var vcmdType = typeof(ViewEventCommand<,>).MakeGenericType(typeof(TViewModel), commandType);

            var args = new object[]
            {
                cmdName,
                cmdPredicates,
                (Func<TViewModel, IObservable<dynamic>>) (v => v.ChangeTracking.DictionaryChanges),
                (Func<TViewModel, IViewEventCommandParameter>) (v =>
                {


                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            v.CurrentEntity.Value,
                            v.ChangeTracking.Where(z => v.CurrentEntity.Value.PropertyList.FirstOrDefault(x => x.Key == z.Key)?.Value != z.Value).ToDictionary(x => x.Key, x => x.Value)
                        },
                        new StateCommandInfo(v.Process.Id, RevolutionData.Context.Entity.Commands.GetEntity), v.Process,
                        v.Source);
                    v.ChangeTracking.Clear();
                    return msg;
                })
            };


           // ViewModelExtensions.VerifyConstuctorVsParameterArray(vcmdType, args);

            var res = (IViewModelEventCommand<IViewModel, IEvent>)Activator.CreateInstance(vcmdType, args);

            return res;
        }
    }
}
