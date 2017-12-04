using System;
using System.Collections.Generic;
using Common;
using Common.DataEntites;
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
                                                    
                                                return SummaryListViewModelInfo.SummaryListViewModel(v.SystemProcessId,
                                                        DynamicEntityType.DynamicEntityTypes[v.EntityTypeName],v.RelationshipOrdinality, v.Symbol, v.Description, v.Priority,
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
                {
                    "EntityViewModel",
                    (v, vp) =>
                    {
                        try
                        {
                            return EntityDetailsViewModelInfo.EntityDetailsViewModel
                                                    (
                                                        v.SystemProcessId,
                                                        DynamicEntityType.DynamicEntityTypes[v.EntityTypeName],v.RelationshipOrdinality, v.Symbol,
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
                { "CachedViewModel", (v, vp) =>
                {
                    try
                    {

                        return CachedViewModelInfo.CachedViewModel(v.SystemProcessId,
                            DynamicEntityType.DynamicEntityTypes[v.EntityTypeName],v.RelationshipOrdinality, v.Symbol,
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

                }},
            };


       
        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            MainWindowViewModelInfo.MainWindowViewModel,
            ScreenViewModelInfo.ScreenViewModel(1),
            HeaderViewModelInfo.HeaderViewModel(1),
            FooterViewModelInfo.FooterViewModel(1)
        };

        public static readonly List<IViewModelInfo> ProcessCache = new List<IViewModelInfo>
        {
            


        };
    }


}
