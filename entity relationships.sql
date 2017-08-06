USE [MRManager-GenSoft-Template]
GO

DECLARE	@return_value int


EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientDetailsInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientAddressesInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientPhoneNumbersInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientNextOfKinsInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientAddressesInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'INonResidentInfo', @ParentField ='Id', @ChildField ='Id'

EXEC	[dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientVitalsInfo', @ParentField ='Id', @ChildField ='Id'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientVisitInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientInfo',		@ChildEntity = N'IPatientVisitInfo', @ParentField ='Id', @ChildField ='PatientId'



EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientVisitInfo',		@ChildEntity = N'IPatientSyntomInfo', @ParentField ='Id', @ChildField ='PatientVisitId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'ISyntoms',		@ChildEntity = N'IPatientSyntomInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientSyntoms',		@ChildEntity = N'IPatientSyntomInfo', @ParentField ='Id', @ChildField ='Id'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientSyntomInfo',		@ChildEntity = N'ISyntomMedicalSystemInfo', @ParentField ='SyntomId', @ChildField ='SyntomId'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'ISyntomMedicalSystems',		@ChildEntity = N'ISyntomMedicalSystemInfo', @ParentField ='Id', @ChildField ='Id'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IInterviews',		@ChildEntity = N'IInterviewInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'ISyntomMedicalSystemInfo',		@ChildEntity = N'IInterviewInfo', @ParentField ='Id', @ChildField ='SystemId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IInterviewInfo',		@ChildEntity = N'IQuestionResponseOptionInfo', @ParentField ='Id', @ChildField ='InterviewId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IQuestionInfo',		@ChildEntity = N'IQuestionResponseOptionInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IResponseInfo',		@ChildEntity = N'IQuestionResponseOptionInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IQuestions',		@ChildEntity = N'IQuestionInfo', @ParentField ='Id', @ChildField ='Id'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IInterviewInfo',		@ChildEntity = N'IQuestionInfo', @ParentField ='Id', @ChildField ='InterviewId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'ISyntomMedicalSystems',		@ChildEntity = N'ISyntomMedicalSystemInfo', @ParentField ='Id', @ChildField ='Id'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IQuestionInfo',		@ChildEntity = N'IResponseOptionInfo', @ParentField ='Id', @ChildField ='QuestionId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientVisitInfo',		@ChildEntity = N'IResponseOptionInfo', @ParentField ='Id', @ChildField ='PatientVisitId'

EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IPatientSyntomInfo',		@ChildEntity = N'IResponseOptionInfo', @ParentField ='Id', @ChildField ='PatientSyntomId'
EXEC	 [dbo].[InsertEntityRelationshipFromNames] 		@parentEntity = N'IQuestionInfo',		@ChildEntity = N'IResponseOptionInfo', @ParentField ='Id', @ChildField ='QuestionId'


EXEC	 [dbo].[InsertDomainEntityTypeFromName] @Entity = 'IResponseOptionInfo'

GO
