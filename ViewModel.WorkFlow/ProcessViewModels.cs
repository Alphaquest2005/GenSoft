using System.Collections.Generic;
using Interfaces;
using RevolutionData;
using ViewModel.Interfaces;
using ViewModel.WorkFlow.ViewModelInfo;
using RevolutionEntities.ViewModels;

namespace ViewModel.WorkFlow
{
    public class ProcessViewModels
    {
        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            MainWindowViewModelInfo.MainWindowViewModel,
            ScreenViewModelInfo.ScreenViewModel,
            SigninViewModelInfo.SigninViewModel,
            HeaderViewModelInfo.HeaderViewModel,
            FooterViewModelInfo.FooterViewModel,
            //PatientSummaryListViewModelInfo.PatientSummaryListViewModel,
            SummaryListViewModelInfo<IPatientInfo>.SummaryListViewModel(3, "", "Patient List", 0,
                new List<EntityViewModelRelationship>()
                {
                    //new EntityViewModelChildRelationship()
                    //{
                    //    ChildType = typeof(IPatientInfo),
                    //    CurrentParentEntity = "Patient",
                    //    ParentProperty = "Id",
                    //    ChildProperty = "PatientId"
                    //}
                }),
           // PatientDetailsViewModelInfo.PatientDetailsViewModel,

           
            //PatientVisitViewModelInfo.PatientVisitViewModel,
            SummaryListViewModelInfo<IPatientVisitInfo>.SummaryListViewModel(3,  "", "Patient Visits", 2, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientInfo),
                ChildType = typeof(IPatientVisitInfo),
                ViewParentProperty = "Patient",
                ParentProperty = "Id",
                ChildProperty = "PatientId"
            }}),
           // PatientSyntomViewModelInfo.PatientSyntomViewModel,
            SummaryListViewModelInfo<IPatientSyntomInfo>.SummaryListViewModel(3,  "", "Patient Syntoms", 3, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientVisitInfo),
                ChildType = typeof(IPatientSyntomInfo),
                ViewParentProperty = "PatientVisit",
                ParentProperty = "Id",
                ChildProperty = "PatientVisitId"
            }}),

            SummaryListViewModelInfo<ISyntomMedicalSystemInfo>.SummaryListViewModel(3,  "", "Systems", 4, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientSyntomInfo),
                ChildType = typeof(ISyntomMedicalSystemInfo),
                ViewParentProperty = "PatientSyntom",
                ParentProperty = "SyntomId",
                ChildProperty = "SyntomId"
            }}),

            SummaryListViewModelInfo<IInterviewInfo>.SummaryListViewModel(3,  "", "Interviews", 5, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(ISyntomMedicalSystemInfo),
                ChildType = typeof(IInterviewInfo),
                ViewParentProperty = "System",
                ParentProperty = "Id",
                ChildProperty = "SystemId"
            }}),
            //InterviewListViewModelInfo.InterviewListViewModel,
            //QuestionListViewModelInfo.QuestionListViewModel,

            SummaryListViewModelInfo<IQuestionInfo>.SummaryListViewModel(3,  "", "Questions", 6, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IInterviewInfo),
                ChildType = typeof(IQuestionInfo),
                ViewParentProperty = "Interview",
                ParentProperty = "Id",
                ChildProperty = "InterviewId"
            }}),



            EntityDetailsViewModelInfo<IPatientDetailsInfo>.EntityDetailsViewModel(3,  "", "Patient Details", 2,
            parentEntities:new List<ViewModelEntity>()
                                    {
                                    new ViewModelEntity(){EntityType = typeof(IPatientInfo), ViewProperty = "Patient", Property = "Id"},
                                    },
            childEntities:new List<ViewModelEntity>()
                                    {
                                    new ViewModelEntity(){EntityType = typeof(IPatientDetailsInfo), ViewProperty = "PatientDetails"},
                                    new ViewModelEntity(){EntityType = typeof(IPatientAddressesInfo), ViewProperty = "Addresses"},
                                    new ViewModelEntity(){EntityType = typeof(IPatientPhoneNumbersInfo), ViewProperty = "PhoneNumbers"},
                                    new ViewModelEntity(){EntityType = typeof(IPatientNextOfKinsInfo), ViewProperty = "NextOfKins"},
                                    new ViewModelEntity(){EntityType = typeof(INonResidentInfo), ViewProperty = "NonResident"},
                                    }),
            //QuestionaireViewModelInfo.QuestionairenaireViewViewModel,
            EntityDetailsViewModelInfo<IQuestionInfo>.EntityDetailsViewModel(
                    processId: 3,
                    symbol: "",
                    description: "Patient Responses",
                    priority:7,
                    parentEntities:new List<ViewModelEntity>()
                                    {
                                        new ViewModelEntity(){EntityType = typeof(IPatientSyntomInfo), ViewProperty = "PatientSyntom", Property = "PatientSyntomId"},
                                        new ViewModelEntity(){EntityType = typeof(IQuestionInfo), ViewProperty = "Question"},
                                    },
                    childEntities:new List<ViewModelEntity>()
                                    {
                                        new ViewModelEntity(){EntityType = typeof(IResponseOptionInfo), ViewProperty = "Responses"},
                                    }
                    ),

            //PatientVitalsViewModelInfo.PatientVitalsViewModel,
            EntityDetailsViewModelInfo<IPatientVitalsInfo>.EntityDetailsViewModel(3,   "", "Vitals", 1, 
            parentEntities:new List<ViewModelEntity>()
            {
            new ViewModelEntity(){EntityType = typeof(IPatientInfo), ViewProperty = "Patient", Property = "Id"},
            },
            childEntities:new List<ViewModelEntity>()
            {
            new ViewModelEntity(){EntityType = typeof(IPatientVitalsInfo), ViewProperty = "PatientVitals"},
            }),

            EntityCacheViewModelInfo<ISyntomPriority>.CacheViewModel(3),
            EntityCacheViewModelInfo<ISyntomStatus>.CacheViewModel(3),
            EntityCacheViewModelInfo<ISyntoms>.CacheViewModel(3),
            EntityCacheViewModelInfo<IVisitType>.CacheViewModel(3),
            EntityCacheViewModelInfo<IPhase>.CacheViewModel(3),
            EntityCacheViewModelInfo<IMedicalCategory>.CacheViewModel(3),
            EntityCacheViewModelInfo<IMedicalSystems>.CacheViewModel(3),
            EntityCacheViewModelInfo<IQuestionResponseTypes>.CacheViewModel(3),
            EntityCacheViewModelInfo<ISex>.CacheViewModel(3),

            EntityViewCacheViewModelInfo<IDoctorInfo>.CacheViewModel(3),
            EntityViewCacheViewModelInfo<ISyntomMedicalSystemInfo>.CacheViewModel(3),
            

        };

        public static readonly List<IViewModelInfo> ProcessCache = new List<IViewModelInfo>
        {
            


        };
    }


}
