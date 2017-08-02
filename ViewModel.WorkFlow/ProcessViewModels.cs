using System.Collections.Generic;
using Interfaces;
using RevolutionData;
using ViewModel.Interfaces;
using ViewModel.WorkFlow.ViewModelInfo;

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
            SummaryListViewModelInfo<IPatientInfo>.SummaryListViewModel(3, "", "Patient List", 0, new List<EntityViewModelRelationship>()),
            PatientDetailsViewModelInfo.PatientDetailsViewModel,
            PatientVitalsViewModelInfo.PatientVitalsViewModel,
            //PatientVisitViewModelInfo.PatientVisitViewModel,
            SummaryListViewModelInfo<IPatientVisitInfo>.SummaryListViewModel(3,  "", "Patient Visits", 2, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientInfo),
                CurrentParentEntity = "Patient",
                ParentProperty = "Id",
                ChildProperty = "PatientId"
            }}),
           // PatientSyntomViewModelInfo.PatientSyntomViewModel,
            SummaryListViewModelInfo<IPatientSyntomInfo>.SummaryListViewModel(3,  "", "Patient Syntoms", 3, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientVisitInfo),
                CurrentParentEntity = "PatientVisit",
                ParentProperty = "Id",
                ChildProperty = "PatientVisitId"
            }}),

            SummaryListViewModelInfo<ISyntomMedicalSystemInfo>.SummaryListViewModel(3,  "", "Systems", 4, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IPatientSyntomInfo),
                CurrentParentEntity = "PatientSyntom",
                ParentProperty = "SyntomId",
                ChildProperty = "SyntomId"
            }}),

            SummaryListViewModelInfo<IInterviewInfo>.SummaryListViewModel(3,  "", "Interviews", 5, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(ISyntomMedicalSystemInfo),
                CurrentParentEntity = "System",
                ParentProperty = "Id",
                ChildProperty = "SystemId"
            }}),
            //InterviewListViewModelInfo.InterviewListViewModel,
            //QuestionListViewModelInfo.QuestionListViewModel,

            SummaryListViewModelInfo<IQuestionInfo>.SummaryListViewModel(3,  "", "Questions", 6, new List<EntityViewModelRelationship>(){new EntityViewModelRelationship()
            {
                ParentType = typeof(IInterviewInfo),
                CurrentParentEntity = "Interview",
                ParentProperty = "Id",
                ChildProperty = "InterviewId"
            }}),

            QuestionaireViewModelInfo.QuestionairenaireViewViewModel,
            
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
