using System.Collections.Generic;
using SystemInterfaces;
using RevolutionData;
using ViewModel.Interfaces;
using RevolutionEntities.ViewModels;

namespace ViewModel.WorkFlow
{
    public class ProcessViewModels
    {
        public static readonly List<IViewModelInfo> ProcessViewModelInfos = new List<IViewModelInfo>
        {
            MainWindowViewModelInfo.MainWindowViewModel,
            ScreenViewModelInfo.ScreenViewModel,
            //SigninViewModelInfo.SigninViewModel,
            EntityDetailsViewModelInfo.EntityDetailsViewModel(2,"ISignInInfo",  "", "Sign In", 0,
                parentEntities:new List<ViewModelEntity>()
                {
                    
                },
                childEntities:new List<ViewModelEntity>()
                {
                    new ViewModelEntity(){EntityType = "ISignInInfo", ViewProperty = "SignInInfo", Property = "Id"},
                }),
            HeaderViewModelInfo.HeaderViewModel,
            FooterViewModelInfo.FooterViewModel,
            //PatientSummaryListViewModelInfo.PatientSummaryListViewModel,
            SummaryListViewModelInfo.SummaryListViewModel(3,"IPatientInfo", "", "Patient List", 0,
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
            SummaryListViewModelInfo.SummaryListViewModel(3,"IPatientVisitInfo" , "", "Patient Visits", 2, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = "IPatientInfo",
                ChildType = "IPatientVisitInfo",
                ViewParentProperty = "Patient",
                ParentProperty = "Id",
                ChildProperty = "PatientId"
            }}),
           // PatientSyntomViewModelInfo.PatientSyntomViewModel,
            SummaryListViewModelInfo.SummaryListViewModel(3,"IPatientSyntomInfo",  "", "Patient Syntoms", 3, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = "IPatientVisitInfo",
                ChildType = "IPatientSyntomInfo",
                ViewParentProperty = "PatientVisit",
                ParentProperty = "Id",
                ChildProperty = "PatientVisitId"
            }}),

            SummaryListViewModelInfo.SummaryListViewModel(3,"ISyntomMedicalSystemInfo",  "", "Systems", 4, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = "IPatientSyntomInfo",
                ChildType = "ISyntomMedicalSystemInfo",
                ViewParentProperty = "PatientSyntom",
                ParentProperty = "SyntomId",
                ChildProperty = "SyntomId"
            }}),

            SummaryListViewModelInfo.SummaryListViewModel(3,"IInterviewInfo",  "", "Interviews", 5, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = "ISyntomMedicalSystemInfo",
                ChildType = "IInterviewInfo",
                ViewParentProperty = "System",
                ParentProperty = "Id",
                ChildProperty = "SystemId"
            }}),
            //InterviewListViewModelInfo.InterviewListViewModel,
            //QuestionListViewModelInfo.QuestionListViewModel,

            SummaryListViewModelInfo.SummaryListViewModel(3,"IQuestionInfo",  "", "Questions", 6, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = "IInterviewInfo",
                ChildType = "IQuestionInfo",
                ViewParentProperty = "Interview",
                ParentProperty = "Id",
                ChildProperty = "InterviewId"
            }}),



            EntityDetailsViewModelInfo.EntityDetailsViewModel(3,"IPatientDetailsInfo",  "", "Patient Details", 2,
            parentEntities:new List<ViewModelEntity>()
                                    {
                                    new ViewModelEntity(){EntityType = "IPatientInfo", ViewProperty = "Patient", Property = "Id"},
                                    },
            childEntities:new List<ViewModelEntity>()
                                    {
                                    new ViewModelEntity(){EntityType = "IPatientDetailsInfo", ViewProperty = "PatientDetails"},
                                    new ViewModelEntity(){EntityType = "IPatientAddressesInfo", ViewProperty = "Addresses"},
                                    new ViewModelEntity(){EntityType = "IPatientPhoneNumbersInfo", ViewProperty = "PhoneNumbers"},
                                    new ViewModelEntity(){EntityType = "IPatientNextOfKinsInfo", ViewProperty = "NextOfKins"},
                                    new ViewModelEntity(){EntityType = "INonResidentInfo", ViewProperty = "NonResident"},
                                    }),
            //PatientVitalsViewModelInfo.PatientVitalsViewModel,
            EntityDetailsViewModelInfo.EntityDetailsViewModel(3,"IPatientVitalsInfo",   "", "Vitals", 1,
                parentEntities:new List<ViewModelEntity>()
                {
                    new ViewModelEntity(){EntityType = "IPatientInfo", ViewProperty = "Patient", Property = "Id"},
                },
                childEntities:new List<ViewModelEntity>()
                {
                    new ViewModelEntity(){EntityType = "IPatientVitalsInfo", ViewProperty = "PatientVitals"},
                }),

            //QuestionaireViewModelInfo.QuestionairenaireViewViewModel,
            EntityDetailsViewModelInfo.EntityDetailsViewModel(
                    processId: 3,
                    entityType:"IQuestionInfo",
                    symbol: "",
                    description: "Patient Responses",
                    priority:7,
                    parentEntities:new List<ViewModelEntity>()
                                    {
                                        new ViewModelEntity(){EntityType = "IPatientSyntomInfo", ViewProperty = "PatientSyntom", Property = "PatientSyntomId"},
                                        new ViewModelEntity(){EntityType = "IQuestionInfo", ViewProperty = "Question"},
                                    },
                    childEntities:new List<ViewModelEntity>()
                                    {
                                        new ViewModelEntity(){EntityType = "IResponseOptionInfo", ViewProperty = "Responses"},
                                    }
                    ),

            

            EntityCacheViewModelInfo.CacheViewModel(3,"ISyntomPriority"),
            EntityCacheViewModelInfo.CacheViewModel(3,"ISyntomPriority"),
            EntityCacheViewModelInfo.CacheViewModel(3,"ISyntoms"),
            EntityCacheViewModelInfo.CacheViewModel(3,"IVisitType"),
            EntityCacheViewModelInfo.CacheViewModel(3,"IPhase"),
            EntityCacheViewModelInfo.CacheViewModel(3,"IMedicalCategory"),
            //EntityCacheViewModelInfo.CacheViewModel(3,""),
            EntityCacheViewModelInfo.CacheViewModel(3,"IQuestionResponseTypes"),
            EntityCacheViewModelInfo.CacheViewModel(3,"ISex"),

            EntityCacheViewModelInfo.CacheViewModel(3,"IDoctorInfo"),
            EntityCacheViewModelInfo.CacheViewModel(3,"ISyntomMedicalSystemInfo"),
            

        };

        public static readonly List<IViewModelInfo> ProcessCache = new List<IViewModelInfo>
        {
            


        };
    }


}
