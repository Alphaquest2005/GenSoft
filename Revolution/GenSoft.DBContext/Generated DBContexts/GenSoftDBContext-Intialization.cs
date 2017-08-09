﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DBContext.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Data;
using GenSoft.Entities;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace GenSoft.DBContexts
{
	public partial class GenSoftDBContext
	{
		private static readonly GenSoftDBContext _instance = new GenSoftDBContext();

		public static GenSoftDBContext Instance => _instance;

		static GenSoftDBContext()
		{
			if (System.ComponentModel.LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
			Instance.Database.EnsureDeleted();
			Instance.Database.EnsureCreated();
			CreateSeedData();
		}

		private static void CreateSeedData()
		{
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Action] ON
						Insert Into dbo.[Action] (Id,Name) Values('1','StartProcess')
						Insert Into dbo.[Action] (Id,Name) Values('2','StartNextProcess')
						Insert Into dbo.[Action] (Id,Name) Values('3','CleanUpProcess')
					SET IDENTITY_INSERT dbo.[Action] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Agent] ON
						Insert Into dbo.[Agent] (Id,UserName) Values('0','System')
						Insert Into dbo.[Agent] (Id,UserName) Values('2','joe')
					SET IDENTITY_INSERT dbo.[Agent] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Machine] ON
						Insert Into dbo.[Machine] (Id,MachineName,Processors) Values('1','ALPHAQUEST-PC','8')
					SET IDENTITY_INSERT dbo.[Machine] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ProcessStateTrigger] ON
						Insert Into dbo.[ProcessStateTrigger] (Id,Name) Values('1','All')
						Insert Into dbo.[ProcessStateTrigger] (Id,Name) Values('2','Any')
					SET IDENTITY_INSERT dbo.[ProcessStateTrigger] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[State] ON
						Insert Into dbo.[State] (Id,Name) Values('1','Started')
						Insert Into dbo.[State] (Id,Name) Values('2','Loaded')
						Insert Into dbo.[State] (Id,Name) Values('3','Completed')
					SET IDENTITY_INSERT dbo.[State] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Type] ON
						Insert Into dbo.[Type] (Id,Name) Values('1','IService')
						Insert Into dbo.[Type] (Id,Name) Values('2','Process')
						Insert Into dbo.[Type] (Id,Name) Values('3','ScreenModel')
						Insert Into dbo.[Type] (Id,Name) Values('4','IPatientInfo')
						Insert Into dbo.[Type] (Id,Name) Values('5','int')
						Insert Into dbo.[Type] (Id,Name) Values('6','string')
						Insert Into dbo.[Type] (Id,Name) Values('7','dateTime')
						Insert Into dbo.[Type] (Id,Name) Values('8','IPatientDetailsInfo')
						Insert Into dbo.[Type] (Id,Name) Values('9','IPatientAddressesInfo')
						Insert Into dbo.[Type] (Id,Name) Values('10','IPatientPhoneNumbersInfo')
						Insert Into dbo.[Type] (Id,Name) Values('11','IPatientNextOfKinsInfo')
						Insert Into dbo.[Type] (Id,Name) Values('12','INonResidentInfo')
						Insert Into dbo.[Type] (Id,Name) Values('13','IPatientVitalsInfo')
						Insert Into dbo.[Type] (Id,Name) Values('14','IPatientVisitInfo')
						Insert Into dbo.[Type] (Id,Name) Values('15','IPatientSyntomInfo')
						Insert Into dbo.[Type] (Id,Name) Values('16','ISyntoms')
						Insert Into dbo.[Type] (Id,Name) Values('17','ISyntomMedicalSystemInfo')
						Insert Into dbo.[Type] (Id,Name) Values('18','ISyntomMedicalSystems')
						Insert Into dbo.[Type] (Id,Name) Values('19','IInterviews')
						Insert Into dbo.[Type] (Id,Name) Values('20','IInterviewInfo')
						Insert Into dbo.[Type] (Id,Name) Values('21','IQuestionResponseOptionInfo')
						Insert Into dbo.[Type] (Id,Name) Values('22','IResponseInfo')
						Insert Into dbo.[Type] (Id,Name) Values('23','IResponseOptions')
						Insert Into dbo.[Type] (Id,Name) Values('24','IQuestionInfo')
						Insert Into dbo.[Type] (Id,Name) Values('25','IQuestions')
						Insert Into dbo.[Type] (Id,Name) Values('26','ISyntomPriority')
						Insert Into dbo.[Type] (Id,Name) Values('27','ISyntomStatus')
						Insert Into dbo.[Type] (Id,Name) Values('28','IVisitType')
						Insert Into dbo.[Type] (Id,Name) Values('29','IPhase')
						Insert Into dbo.[Type] (Id,Name) Values('30','IMedicalCategory')
						Insert Into dbo.[Type] (Id,Name) Values('31','IMedicalSystems')
						Insert Into dbo.[Type] (Id,Name) Values('32','IQuestionResponseTypes')
						Insert Into dbo.[Type] (Id,Name) Values('33','ISex')
						Insert Into dbo.[Type] (Id,Name) Values('34','IDoctorInfo')
						Insert Into dbo.[Type] (Id,Name) Values('35','IPatientSyntoms')
						Insert Into dbo.[Type] (Id,Name) Values('36','IResponseOptionInfo')
						Insert Into dbo.[Type] (Id,Name) Values('37','IPatient')
					SET IDENTITY_INSERT dbo.[Type] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityType] (Id) Values('1')
						Insert Into dbo.[EntityType] (Id) Values('2')
						Insert Into dbo.[EntityType] (Id) Values('3')
						Insert Into dbo.[EntityType] (Id) Values('4')
						Insert Into dbo.[EntityType] (Id) Values('8')
						Insert Into dbo.[EntityType] (Id) Values('9')
						Insert Into dbo.[EntityType] (Id) Values('10')
						Insert Into dbo.[EntityType] (Id) Values('11')
						Insert Into dbo.[EntityType] (Id) Values('12')
						Insert Into dbo.[EntityType] (Id) Values('13')
						Insert Into dbo.[EntityType] (Id) Values('14')
						Insert Into dbo.[EntityType] (Id) Values('15')
						Insert Into dbo.[EntityType] (Id) Values('16')
						Insert Into dbo.[EntityType] (Id) Values('17')
						Insert Into dbo.[EntityType] (Id) Values('18')
						Insert Into dbo.[EntityType] (Id) Values('19')
						Insert Into dbo.[EntityType] (Id) Values('20')
						Insert Into dbo.[EntityType] (Id) Values('21')
						Insert Into dbo.[EntityType] (Id) Values('22')
						Insert Into dbo.[EntityType] (Id) Values('23')
						Insert Into dbo.[EntityType] (Id) Values('24')
						Insert Into dbo.[EntityType] (Id) Values('25')
						Insert Into dbo.[EntityType] (Id) Values('26')
						Insert Into dbo.[EntityType] (Id) Values('27')
						Insert Into dbo.[EntityType] (Id) Values('28')
						Insert Into dbo.[EntityType] (Id) Values('29')
						Insert Into dbo.[EntityType] (Id) Values('30')
						Insert Into dbo.[EntityType] (Id) Values('31')
						Insert Into dbo.[EntityType] (Id) Values('32')
						Insert Into dbo.[EntityType] (Id) Values('33')
						Insert Into dbo.[EntityType] (Id) Values('34')
						Insert Into dbo.[EntityType] (Id) Values('35')
						Insert Into dbo.[EntityType] (Id) Values('36')
						Insert Into dbo.[EntityType] (Id) Values('37')
");
			//No test data for ActionEntityType
			//No test data for ActionEntityType
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Process] ON
						Insert Into dbo.[Process] (UserId,Id,Description,Name,ParentProcessId,Symbol) Values('0','1','Prepare system for Intial Use','Starting System','0','Start')
						Insert Into dbo.[Process] (UserId,Id,Description,Name,ParentProcessId,Symbol) Values('0','2','User Login','User SignOn','1','User')
						Insert Into dbo.[Process] (UserId,Id,Description,Name,ParentProcessId,Symbol) Values('0','3','User Screen','Load User Screen','2','UserScreen')
					SET IDENTITY_INSERT dbo.[Process] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[SourceType] (Id) Values('1')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[MessageSource] ON
						Insert Into dbo.[MessageSource] (MachineId,Id,SourceGuid,Name,SourceTypeId,ProcessId) Values('1','1','575C9BC4-F78F-49F7-AD10-C1F47277A23B','ServiceManager','1','1')
					SET IDENTITY_INSERT dbo.[MessageSource] OFF");
			//No test data for Message
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Entity] ON
						Insert Into dbo.[Entity] (Id,EntityTypeId,EntryDateTimeStamp) Values('1','1',cast((select Value from AmoebaDB.dbo.TestValues where Id = 115897) as varbinary(max)))
						Insert Into dbo.[Entity] (Id,EntityTypeId,EntryDateTimeStamp) Values('2','3',cast((select Value from AmoebaDB.dbo.TestValues where Id = 115900) as varbinary(max)))
						Insert Into dbo.[Entity] (Id,EntityTypeId,EntryDateTimeStamp) Values('3','37',cast((select Value from AmoebaDB.dbo.TestValues where Id = 115903) as varbinary(max)))
					SET IDENTITY_INSERT dbo.[Entity] OFF");
			//No test data for Message
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ProcessState] ON
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('1','ServiceManagerStarted','1','1')
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('2','Process0Started','1','1')
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('3','ScreenViewCreated','1','1')
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('4','ScreenViewLoaded','1','2')
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('5','Process0Completed','1','3')
						Insert Into dbo.[ProcessState] (Id,Name,ProcessId,StateId) Values('6','Process3Started','3','1')
					SET IDENTITY_INSERT dbo.[ProcessState] OFF");
			//No test data for Message
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[ProcessComplexState] (Id,StateTriggerId) Values('1','1')
						Insert Into dbo.[ProcessComplexState] (Id,StateTriggerId) Values('5','1')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ProcessComplexStateExpectedProcessState] ON
						Insert Into dbo.[ProcessComplexStateExpectedProcessState] (ComplexStateId,Id,ProcessStateId) Values('1','1','1')
						Insert Into dbo.[ProcessComplexStateExpectedProcessState] (ComplexStateId,Id,ProcessStateId) Values('5','7','2')
						Insert Into dbo.[ProcessComplexStateExpectedProcessState] (ComplexStateId,Id,ProcessStateId) Values('5','2','3')
						Insert Into dbo.[ProcessComplexStateExpectedProcessState] (ComplexStateId,Id,ProcessStateId) Values('5','3','4')
					SET IDENTITY_INSERT dbo.[ProcessComplexStateExpectedProcessState] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[DomainEntityType] (Id) Values('4')
						Insert Into dbo.[DomainEntityType] (Id) Values('8')
						Insert Into dbo.[DomainEntityType] (Id) Values('9')
						Insert Into dbo.[DomainEntityType] (Id) Values('10')
						Insert Into dbo.[DomainEntityType] (Id) Values('11')
						Insert Into dbo.[DomainEntityType] (Id) Values('12')
						Insert Into dbo.[DomainEntityType] (Id) Values('13')
						Insert Into dbo.[DomainEntityType] (Id) Values('14')
						Insert Into dbo.[DomainEntityType] (Id) Values('15')
						Insert Into dbo.[DomainEntityType] (Id) Values('16')
						Insert Into dbo.[DomainEntityType] (Id) Values('17')
						Insert Into dbo.[DomainEntityType] (Id) Values('18')
						Insert Into dbo.[DomainEntityType] (Id) Values('19')
						Insert Into dbo.[DomainEntityType] (Id) Values('20')
						Insert Into dbo.[DomainEntityType] (Id) Values('21')
						Insert Into dbo.[DomainEntityType] (Id) Values('22')
						Insert Into dbo.[DomainEntityType] (Id) Values('23')
						Insert Into dbo.[DomainEntityType] (Id) Values('24')
						Insert Into dbo.[DomainEntityType] (Id) Values('25')
						Insert Into dbo.[DomainEntityType] (Id) Values('26')
						Insert Into dbo.[DomainEntityType] (Id) Values('27')
						Insert Into dbo.[DomainEntityType] (Id) Values('28')
						Insert Into dbo.[DomainEntityType] (Id) Values('29')
						Insert Into dbo.[DomainEntityType] (Id) Values('30')
						Insert Into dbo.[DomainEntityType] (Id) Values('31')
						Insert Into dbo.[DomainEntityType] (Id) Values('32')
						Insert Into dbo.[DomainEntityType] (Id) Values('33')
						Insert Into dbo.[DomainEntityType] (Id) Values('34')
						Insert Into dbo.[DomainEntityType] (Id) Values('35')
						Insert Into dbo.[DomainEntityType] (Id) Values('36')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ProcessStateDomainEntityTypes] ON
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('4','1','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('8','2','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('9','5','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('10','6','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('11','7','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('12','8','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('13','9','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('14','10','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('15','11','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('16','12','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('17','13','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('18','14','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('19','15','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('20','16','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('21','17','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('22','18','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('23','19','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('24','20','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('25','21','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('26','22','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('27','23','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('28','24','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('29','25','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('30','26','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('31','27','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('32','28','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('33','29','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('34','30','6')
						Insert Into dbo.[ProcessStateDomainEntityTypes] (DomainEntityTypeId,Id,ProcessStateId) Values('36','31','6')
					SET IDENTITY_INSERT dbo.[ProcessStateDomainEntityTypes] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[ProcessStateInfo] (Id,Description,Notes) Values('1','Service Manager Started','Service Manager Started')
						Insert Into dbo.[ProcessStateInfo] (Id,Description,Notes) Values('2','ProcessStarted','Process 0 Started ')
						Insert Into dbo.[ProcessStateInfo] (Id,Description,Notes) Values('3','ScreenView Created','This view contains all views')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[StateAction] ON
						Insert Into dbo.[StateAction] (ActionId,Id,ProcessStateId) Values('1','5','1')
						Insert Into dbo.[StateAction] (ActionId,Id,ProcessStateId) Values('3','6','5')
					SET IDENTITY_INSERT dbo.[StateAction] OFF");
			//No test data for Command
			//No test data for Command
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[StateActionExpectedProcessState] (ExpectedProcesStateId,Id) Values('2','5')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[User] (Id,Password) Values('2','test')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[DataType] (Id) Values('5')
						Insert Into dbo.[DataType] (Id) Values('6')
						Insert Into dbo.[DataType] (Id) Values('7')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Attributes] ON
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('1','Id','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('2','FirstName','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('3','Address','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('4','PhoneNumber','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('6','Sex','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('7','BirthCountry','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('8','Email','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('9','BirthDate','7')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('11','EmailAddress','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('12','Marital Status','6')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('40','PatientId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('41','SyntomId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('43','InterviewId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('45','PatientVisitId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('46','MedicalSystemId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('48','PatientSyntomId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('49','QuestionId','5')
						Insert Into dbo.[Attributes] (Id,Name,DataTypeId) Values('50','LastName','6')
					SET IDENTITY_INSERT dbo.[Attributes] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[EntityAttribute] ON
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('1','1','3','1')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('2','2','3','Jonali')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('50','3','3','St. Louis')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('3','4','3','Fort Jeudy')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('4','5','3','456-4724')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('9','6','3','3/16/1994')
						Insert Into dbo.[EntityAttribute] (AttributeId,Id,EntityId,Value) Values('6','7','3','Female')
					SET IDENTITY_INSERT dbo.[EntityAttribute] OFF");
			//No test data for EntityAttributeChanges
			//No test data for EntityAttributeChanges
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityId] (Id) Values('1')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityName] (Id) Values('2')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[EntityTypeAttributes] ON
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('1','1','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('2','2','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('3','3','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('4','4','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('6','6','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('7','7','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('8','8','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('9','9','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('11','11','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('12','12','37')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('1','13','4')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('2','14','4')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('3','15','4')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('4','16','4')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('1','18','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('2','19','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('3','20','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('4','21','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('6','23','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('7','24','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('8','25','8')
						Insert Into dbo.[EntityTypeAttributes] (AttributeId,Id,EntityTypeId) Values('9','26','8')
					SET IDENTITY_INSERT dbo.[EntityTypeAttributes] OFF");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[EntityRelationships] ON
						Insert Into dbo.[EntityRelationships] (Id,ParentEntityId,ChildEntityId) Values('67','13','18')
					SET IDENTITY_INSERT dbo.[EntityRelationships] OFF");
			//No test data for MessageType
			//No test data for MessageType
			//No test data for TypeParameter
			//No test data for TypeParameter
			//No test data for ActionEntityType
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ApplicationSetting] ON
						Insert Into dbo.[ApplicationSetting] (Id,AutoRun) Values('1','1')
					SET IDENTITY_INSERT dbo.[ApplicationSetting] OFF");
			//No test data for Command
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[CompositeRequest] (Id) Values('36')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[DomainEntityCache] (Id) Values('26')
						Insert Into dbo.[DomainEntityCache] (Id) Values('27')
						Insert Into dbo.[DomainEntityCache] (Id) Values('28')
						Insert Into dbo.[DomainEntityCache] (Id) Values('29')
						Insert Into dbo.[DomainEntityCache] (Id) Values('30')
						Insert Into dbo.[DomainEntityCache] (Id) Values('31')
						Insert Into dbo.[DomainEntityCache] (Id) Values('32')
						Insert Into dbo.[DomainEntityCache] (Id) Values('33')
						Insert Into dbo.[DomainEntityCache] (Id) Values('34')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[DomainEntityTypeSourceEntity] (Id,SourceEntity) Values('4','Patient')
						Insert Into dbo.[DomainEntityTypeSourceEntity] (Id,SourceEntity) Values('8','Patient')
						Insert Into dbo.[DomainEntityTypeSourceEntity] (Id,SourceEntity) Values('12','NonResident')
						Insert Into dbo.[DomainEntityTypeSourceEntity] (Id,SourceEntity) Values('13','Vitals')
");
			//No test data for EntityAttributeChanges
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityList] (Id) Values('4')
						Insert Into dbo.[EntityList] (Id) Values('14')
						Insert Into dbo.[EntityList] (Id) Values('15')
						Insert Into dbo.[EntityList] (Id) Values('17')
						Insert Into dbo.[EntityList] (Id) Values('20')
						Insert Into dbo.[EntityList] (Id) Values('24')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('4','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('8','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('9','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('10','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('11','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('12','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('13','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('14','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('15','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('17','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('20','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('21','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('22','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('24','37')
						Insert Into dbo.[EntityView] (Id,BaseEntityTypeId) Values('34','37')
");
			//No test data for Event
			//No test data for Message
			//No test data for MessageType
			//No test data for TypeParameter
			//No test data for ViewModel
		}
               
			
		
	}
}
