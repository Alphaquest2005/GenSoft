using System;
using System.Collections.Generic;
using Common;
using DomainUtilities;
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
                                                    DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(v.EntityTypeName);
                                                return SummaryListViewModelInfo.SummaryListViewModel(v.SystemProcess,
                                                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(v.EntityTypeName),v.RelationshipOrdinality, v.Symbol, v.Description, v.Priority,
                                                        v.EntityViewModelRelationships,
                                                        v.EntityTypeViewModelCommands,
                                                        vp);
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e);
                                                    throw;
                                                }
                                                
                                            }},
                { "EntityViewModel",(v, vp) =>
                    {
                        try
                        {
                            DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(v.EntityTypeName);
                            return EntityDetailsViewModelInfo.EntityDetailsViewModel
                                                    (
                                                        v.SystemProcess,
                                                        DynamicEntityTypeExtensions.GetOrAddDynamicEntityType(v.EntityTypeName),v.RelationshipOrdinality, v.Symbol,
                                                        v.Description, v.Priority,
                                                        viewRelationships: v.EntityViewModelRelationships,
                                                        viewCommands: v.EntityTypeViewModelCommands,
                                                        displayProperties: vp);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        
                    }
                },
               
            };

        


        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            HeaderViewModelInfo.HeaderViewModel(),
            FooterViewModelInfo.FooterViewModel(),
            ScreenViewModelInfo.ScreenViewModel(),
        };

        
    }


}
