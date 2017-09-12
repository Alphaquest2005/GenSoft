using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Akka.Util.Internal;
using Common;
using Common.DataEntites;
using GenSoft.DBContexts;
using GenSoft.Entities;
using GenSoft.Interfaces;
using MoreLinq;
using RevolutionData;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;

namespace ViewModel.WorkFlow
{
    public class ProcessViewModels
    {

        public static Dictionary<string, Func<EntityTypeViewModel, ViewAttributeDisplayProperties, IViewModelInfo>> ProcessViewModelFactory =
            new Dictionary<string, Func<EntityTypeViewModel, ViewAttributeDisplayProperties, IViewModelInfo>>()
            {
                { "SummaryListViewModel", (v, vp) =>
                                            {
                                                try
                                                {
                                                    
                                                return SummaryListViewModelInfo.SummaryListViewModel(v.ProcessStateDomainEntityTypes.ProcessState.ProcessId,
                                                        DynamicEntityType.DynamicEntityTypes[v.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name], v.Symbol, v.Description, v.Priority,
                                                        v.ProcessStateDomainEntityTypes
                                                            .DomainEntityType
                                                            .EntityType
                                                            .EntityTypeAttributes
                                                            .SelectMany(x => x.ChildEntitys)
                                                            .Where(x => x.ChildEntity.EntityType.Type.Name == v.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name
                                                                        && DynamicEntityType.DynamicEntityTypes.ContainsKey(x.ParentEntity.EntityType.Type.Name))
                                                            .DistinctBy(x => x.Id)
                                                            .Select(x => new EntityViewModelRelationship()
                                                            {
                                                                ParentType = x.ParentEntity.EntityType.Type.Name,
                                                                ChildType = x.ChildEntity.EntityType.Type.Name,
                                                                ParentProperty = x.ParentEntity.Attributes.Name,
                                                                ChildProperty = x.ChildEntity.Attributes.Name,

                                                            }).ToList(),
                                                            //.SelectMany(x => x.ParentEntitys).DistinctBy(x => x.Id)
                                                            //.Select(x => new EntityViewModelRelationship()
                                                            //{
                                                            //    ParentType = x.ParentEntity.EntityType.Type.Name,
                                                            //    ChildType = x.ChildEntity.EntityType.Type.Name,
                                                            //    ParentProperty = x.ParentEntity.Attributes.Name,
                                                            //    ChildProperty = x.ChildEntity.Attributes.Name,
                                                                
                                                            //}).ToList(),
                                                        v.EntityViewModelCommands.DistinctBy(x => x.Id).ToList(),
                                                        vp);
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e);
                                                    throw;
                                                }
                                                
                                            }},
                {
                    "EntityViewModel",
                    (v, vp) =>
                    {
                        try
                        {
                            return EntityDetailsViewModelInfo.EntityDetailsViewModel
                                                    (
                                                        v.ProcessStateDomainEntityTypes.ProcessState.ProcessId,
                                                        DynamicEntityType.DynamicEntityTypes[
                                                            v.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name], v.Symbol,
                                                        v.Description, v.Priority,
                                                        viewRelationships: v.ProcessStateDomainEntityTypes
                                                            .DomainEntityType
                                                            .EntityType
                                                            .EntityTypeAttributes
                                                            .SelectMany(x => x.ParentEntitys).DistinctBy(x => x.ChildEntityId)
                                                            .Select(x => new EntityViewModelRelationship()
                                                            {
                                                                ParentType = x.ParentEntity?.EntityType.Type.Name,
                                                                ChildType = x.ChildEntity?.EntityType.Type.Name,
                                                                ParentProperty = x.ParentEntity?.Attributes.Name,
                                                                ChildProperty = x.ChildEntity?.Attributes.Name,

                                                            }).ToList(),
                                                       
                                                        viewCommands: v.EntityViewModelCommands.DistinctBy(x => x.Id).ToList(),
                                                        displayProperties: vp);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        
                    }
                }
            };


       
        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            MainWindowViewModelInfo.MainWindowViewModel,
            ScreenViewModelInfo.ScreenViewModel(1),
            HeaderViewModelInfo.HeaderViewModel(3),
            FooterViewModelInfo.FooterViewModel(3)
        };

        public static readonly List<IViewModelInfo> ProcessCache = new List<IViewModelInfo>
        {
            


        };
    }


}
