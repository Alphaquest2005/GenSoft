using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using GenSoft.Entities;
using RevolutionEntities.ViewModels;
using ViewModel.Interfaces;
using ViewModelInterfaces;
using IEvent = SystemInterfaces.IEvent;

namespace ViewModel.WorkFlow
{
    public static class ViewModelInfoExtensions
    {
        public static IViewModelEventCommand<IViewModel, IEvent> CreateCustomCommand<TViewModel>(ViewModelCommand cmd, List<EntityViewModelRelationship> parentEntites) where TViewModel : IEntityViewModel
        {
            var cmdPredicates = new List<Func<TViewModel, bool>>(){v => v.CurrentEntity.Value != null};
            if (cmd.ExistingEntities) cmdPredicates.Add(v => v.CurrentEntity.Value?.Id != 0);
            else cmdPredicates.Add(v => v.CurrentEntity.Value?.Id <= 0);

            if (cmd.RequireAllFields) cmdPredicates.Add(v => v.ChangeTracking.Count == v.CurrentEntity.Value.PropertyList.Where(x => x.IsComputed == false).Count(x => x.Key != nameof(IDynamicEntity.Id)));
            else cmdPredicates.Add(v => v.ChangeTracking.Any(z => v.CurrentEntity.Value.PropertyList.FirstOrDefault(x => x.Key == z.Key)?.Value.ToString() != (z.Value is IEntityKeyValuePair ? ((IEntityKeyValuePair)z.Value).Value.ToString() : z.Value.ToString())));

            var commandType = typeof(SystemInterfaces.IEntity).Assembly.GetType($"SystemInterfaces.{cmd.CommandType.Name}");
            var vcmdType = typeof(ViewEventCommand<,>).MakeGenericType(typeof(TViewModel), commandType);

            var args = new object[]
            {
                cmd.Name,
                cmdPredicates,
                (Func<TViewModel, IObservable<dynamic>>) (v => v.ChangeTracking.DictionaryChanges),
                (Func<TViewModel, IViewEventCommandParameter>) (v =>
                {

                    //foreach (var p in parentEntites.Where(x => x.ParentType != null))
                    //{
                    //    var parentEntity = v.ParentEntities.FirstOrDefault(x => x.EntityType.Name == p.ParentType);
                    //    if (parentEntity == null) continue;
                    //    v.ChangeTracking.AddOrUpdate(p.ChildProperty, parentEntity.Id);
                    //}

                    var msg = new ViewEventCommandParameter(
                        new object[]
                        {
                            v.CurrentEntity.Value,
                            v.ChangeTracking.Where(z => v.CurrentEntity.Value.PropertyList.FirstOrDefault(x => x.Key == z.Key)?.Value?.ToString() != (z.Value is IEntityKeyValuePair ? ((IEntityKeyValuePair)z.Value).Value?.ToString() : z.Value?.ToString())).ToDictionary(x => x.Key, x => (x.Value is IEntityKeyValuePair? ((IEntityKeyValuePair)x.Value).Value.ToString() : x.Value.ToString()))
                        },
                        new RevolutionEntities.Process.StateCommandInfo(v.Process, RevolutionData.Context.Entity.Commands.GetEntity), v.Process,
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
