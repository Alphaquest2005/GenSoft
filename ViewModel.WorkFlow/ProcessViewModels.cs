using System;
using System.Collections.Generic;
using System.Linq;
using SystemInterfaces;
using Common.DataEntites;
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

        public static Dictionary<string, Func<EntityTypeViewModel, IViewModelInfo>> ProcessViewModelFactory =
            new Dictionary<string, Func<EntityTypeViewModel, IViewModelInfo>>()
            {
                { "SummaryListViewModel", v =>
                                            {
                                                return SummaryListViewModelInfo.SummaryListViewModel(v.ProcessStateDomainEntityTypes.ProcessState.ProcessId,
                                                    DynamicEntityType.DynamicEntityTypes[v.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name], v.Symbol, v.Description, v.Priority,
                                                    v.ProcessStateDomainEntityTypes
                                                        .DomainEntityType
                                                        .EntityType
                                                        .EntityTypeAttributes
                                                        .SelectMany(x => x.ChildEntitys).DistinctBy(x => x.Id)
                                                        .Select(x => new EntityViewModelRelationship()
                                                        {
                                                            ParentType = x.ParentEntity.EntityType.Type.Name,
                                                            ChildType = x.ChildEntity.EntityType.Type.Name,
                                                            ParentProperty = x.ParentEntity.Attributes.Name,
                                                            ChildProperty = x.ChildEntity.Attributes.Name
                                                        }).ToList(),
                                                    v.EntityViewModelCommands.DistinctBy(x => x.Id).ToList());
                                            }},
                {
                    "EntityViewModel",
                    v =>
                    {
                        return EntityDetailsViewModelInfo.EntityDetailsViewModel
                        (
                            v.ProcessStateDomainEntityTypes.ProcessState.ProcessId,
                            DynamicEntityType.DynamicEntityTypes[
                                v.ProcessStateDomainEntityTypes.DomainEntityType.EntityType.Type.Name], v.Symbol,
                            v.Description, v.Priority,
                            parentEntities: v.ProcessStateDomainEntityTypes
                                .DomainEntityType
                                .EntityType
                                .EntityTypeAttributes
                                .SelectMany(x => x.ChildEntitys).DistinctBy(x => x.ChildEntityId)
                                .Where(x => DynamicEntityType.DynamicEntityTypes.ContainsKey(x.ParentEntity.EntityType.Type.Name))
                                .Select(x => new ViewModelEntity()
                                {
                                    EntityType = DynamicEntityType.DynamicEntityTypes[x.ParentEntity.EntityType.Type.Name],
                                    ViewProperty = v.PropertyName
                                }).ToList(),
                            childEntities: v.ProcessStateDomainEntityTypes
                                .DomainEntityType
                                .EntityType
                                .EntityTypeAttributes
                                .SelectMany(x => x.ChildEntitys).DistinctBy(x => x.ChildEntityId)
                                .Where(x => DynamicEntityType.DynamicEntityTypes.ContainsKey(x.ChildEntity.EntityType.Type.Name))
                                .Select(x => new ViewModelEntity()
                                {
                                    EntityType = DynamicEntityType.DynamicEntityTypes[x.ChildEntity.EntityType.Type.Name],
                                    ViewProperty = v.PropertyName
                                }).ToList(),
                            viewCommands:v.EntityViewModelCommands.DistinctBy(x => x.Id).ToList()
                         );
                    }
                }
            };

        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            MainWindowViewModelInfo.MainWindowViewModel,
            ScreenViewModelInfo.ScreenViewModel,
            

            HeaderViewModelInfo.HeaderViewModel,
            FooterViewModelInfo.FooterViewModel,


            //SummaryListViewModelInfo.SummaryListViewModel(3,DynamicEntityType.DynamicEntityTypes["IPatientInfo"], "", "Patient List", 0,
            //    new List<EntityViewModelRelationship>()
            //    {
                    
            //    }),

            //SummaryListViewModelInfo.SummaryListViewModel(3,DynamicEntityType.DynamicEntityTypes["IPatientVisitInfo"] , "", "Patient Visits", 2, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            //{
            //    ParentType = "IPatientInfo",
            //    ChildType = "IPatientVisitInfo",
            //    ViewParentProperty = "Patient",
            //    ParentProperty = "Id",
            //    ChildProperty = "PatientId"
            //}}),

            //SummaryListViewModelInfo.SummaryListViewModel(3,"IPatientSyntomInfo",  "", "Patient Syntoms", 3, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            //{
            //    ParentType = "IPatientVisitInfo",
            //    ChildType = "IPatientSyntomInfo",
            //    ViewParentProperty = "PatientVisit",
            //    ParentProperty = "Id",
            //    ChildProperty = "PatientVisitId"
            //}}),

            //SummaryListViewModelInfo.SummaryListViewModel(3,"ISyntomMedicalSystemInfo",  "", "Systems", 4, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            //{
            //    ParentType = "IPatientSyntomInfo",
            //    ChildType = "ISyntomMedicalSystemInfo",
            //    ViewParentProperty = "PatientSyntom",
            //    ParentProperty = "SyntomId",
            //    ChildProperty = "SyntomId"
            //}}),

            //SummaryListViewModelInfo.SummaryListViewModel(3,"IInterviewInfo",  "", "Interviews", 5, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            //{
            //    ParentType = "ISyntomMedicalSystemInfo",
            //    ChildType = "IInterviewInfo",
            //    ViewParentProperty = "System",
            //    ParentProperty = "Id",
            //    ChildProperty = "SystemId"
            //}}),
            ////InterviewListViewModelInfo.InterviewListViewModel,
            ////QuestionListViewModelInfo.QuestionListViewModel,

            //SummaryListViewModelInfo.SummaryListViewModel(3,"IQuestionInfo",  "", "Questions", 6, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            //{
            //    ParentType = "IInterviewInfo",
            //    ChildType = "IQuestionInfo",
            //    ViewParentProperty = "Interview",
            //    ParentProperty = "Id",
            //    ChildProperty = "InterviewId"
            //}}),

            //EntityDetailsViewModelInfo.EntityDetailsViewModel(2,DynamicEntityType.DynamicEntityTypes["ISignInInfo"],  "", "Sign In", 0,
            //    parentEntities:new List<ViewModelEntity>()
            //    {

            //    },
            //    childEntities:new List<ViewModelEntity>()
            //    {
            //        new ViewModelEntity(){EntityType = DynamicEntityType.DynamicEntityTypes["ISignInInfo"], ViewProperty = "SignInInfo", Property = "Id"},
            //    }),

            //EntityDetailsViewModelInfo.EntityDetailsViewModel(3,DynamicEntityType.DynamicEntityTypes["IPatientDetailsInfo"],  "", "Patient Details", 2,
            //parentEntities:new List<ViewModelEntity>()
            //                        {
            //                        new ViewModelEntity(){EntityType = DynamicEntityType.DynamicEntityTypes["IPatientInfo"], ViewProperty = "Patient", Property = "Id"},
            //                        },
            //childEntities:new List<ViewModelEntity>()
            //                        {
            //                        new ViewModelEntity(){EntityType = DynamicEntityType.DynamicEntityTypes["IPatientDetailsInfo"], ViewProperty = "PatientDetails"},
            //                        //new ViewModelEntity(){EntityType = "IPatientAddressesInfo", ViewProperty = "Addresses"},
            //                        //new ViewModelEntity(){EntityType = "IPatientPhoneNumbersInfo", ViewProperty = "PhoneNumbers"},
            //                        //new ViewModelEntity(){EntityType = "IPatientNextOfKinsInfo", ViewProperty = "NextOfKins"},
            //                        //new ViewModelEntity(){EntityType = "INonResidentInfo", ViewProperty = "NonResident"},
            //                        }),

            //EntityDetailsViewModelInfo.EntityDetailsViewModel(3,"IPatientVitalsInfo",   "", "Vitals", 1,
            //    parentEntities:new List<ViewModelEntity>()
            //    {
            //        new ViewModelEntity(){EntityType = "IPatientInfo", ViewProperty = "Patient", Property = "Id"},
            //    },
            //    childEntities:new List<ViewModelEntity>()
            //    {
            //        new ViewModelEntity(){EntityType = "IPatientVitalsInfo", ViewProperty = "PatientVitals"},
            //    }),


            //EntityDetailsViewModelInfo.EntityDetailsViewModel(
            //        processId: 3,
            //        entityType:"IQuestionInfo",
            //        symbol: "",
            //        description: "Patient Responses",
            //        priority:7,
            //        parentEntities:new List<ViewModelEntity>()
            //                        {
            //                            new ViewModelEntity(){EntityType = "IPatientSyntomInfo", ViewProperty = "PatientSyntom", Property = "PatientSyntomId"},
            //                            new ViewModelEntity(){EntityType = "IQuestionInfo", ViewProperty = "Question"},
            //                        },
            //        childEntities:new List<ViewModelEntity>()
            //                        {
            //                            new ViewModelEntity(){EntityType = "IResponseOptionInfo", ViewProperty = "Responses"},
            //                        }
            //        ),






        };

        public static readonly List<IViewModelInfo> ProcessCache = new List<IViewModelInfo>
        {
            


        };
    }


}
