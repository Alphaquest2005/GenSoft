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
");
			//No test data for ActionEntityType
			//No test data for ActionEntityType
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Process] ON
						Insert Into dbo.[Process] (UserId,Description,Id,Name,ParentProcessId,Symbol) Values('0','Prepare system for Intial Use','1','Starting System','0','Start')
						Insert Into dbo.[Process] (UserId,Description,Id,Name,ParentProcessId,Symbol) Values('0','User Login','2','User SignOn','1','User')
						Insert Into dbo.[Process] (UserId,Description,Id,Name,ParentProcessId,Symbol) Values('0','User Screen','3','Load User Screen','2','UserScreen')
					SET IDENTITY_INSERT dbo.[Process] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[SourceType] (Id) Values('1')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[MessageSource] ON
						Insert Into dbo.[MessageSource] (MachineId,Id,Name,ProcessId,SourceGuid,SourceTypeId) Values('1','1','ServiceManager','1','575C9BC4-F78F-49F7-AD10-C1F47277A23B','1')
					SET IDENTITY_INSERT dbo.[MessageSource] OFF");
			//No test data for Message
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[Entity] ON
						Insert Into dbo.[Entity] (Id,EntityTypeId,Name) Values('1','1','IServiceManager')
						Insert Into dbo.[Entity] (Id,EntityTypeId,Name) Values('2','3','IScreenModel')
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
					SET IDENTITY_INSERT dbo.[EntityTypeAttributes] ON
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','4','1','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','2','Name')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','3','Address')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','4','PhoneNumber')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','5','Age')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','6','Sex')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','7','BirthCountry')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','4','8','Email')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('7','4','9','BirthDate')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','8','10','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','8','11','EmailAddress')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('6','8','12','Marital Status')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','9','13','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','10','14','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','11','15','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','12','16','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','13','17','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','14','18','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','15','19','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','16','20','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','17','21','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','18','22','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','19','23','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','20','24','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','21','25','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','22','26','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','23','27','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','24','28','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','25','29','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','26','30','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','27','31','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','28','32','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','29','33','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','30','34','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','31','35','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','32','36','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','33','37','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','34','38','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','35','39','Id')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','14','40','PatientId')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','15','41','SyntomId')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','17','42','SyntomId')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','21','43','InterviewId')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','24','44','InterviewId')
						Insert Into dbo.[EntityTypeAttributes] (DataTypeId,EntityTypeId,Id,Name) Values('5','15','45','PatientVisitId')
					SET IDENTITY_INSERT dbo.[EntityTypeAttributes] OFF");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityId] (Id) Values('1')
						Insert Into dbo.[EntityId] (Id) Values('10')
						Insert Into dbo.[EntityId] (Id) Values('13')
						Insert Into dbo.[EntityId] (Id) Values('14')
						Insert Into dbo.[EntityId] (Id) Values('15')
						Insert Into dbo.[EntityId] (Id) Values('16')
						Insert Into dbo.[EntityId] (Id) Values('17')
						Insert Into dbo.[EntityId] (Id) Values('18')
						Insert Into dbo.[EntityId] (Id) Values('19')
						Insert Into dbo.[EntityId] (Id) Values('20')
						Insert Into dbo.[EntityId] (Id) Values('21')
						Insert Into dbo.[EntityId] (Id) Values('22')
						Insert Into dbo.[EntityId] (Id) Values('23')
						Insert Into dbo.[EntityId] (Id) Values('24')
						Insert Into dbo.[EntityId] (Id) Values('25')
						Insert Into dbo.[EntityId] (Id) Values('26')
						Insert Into dbo.[EntityId] (Id) Values('27')
						Insert Into dbo.[EntityId] (Id) Values('28')
						Insert Into dbo.[EntityId] (Id) Values('29')
						Insert Into dbo.[EntityId] (Id) Values('30')
						Insert Into dbo.[EntityId] (Id) Values('31')
						Insert Into dbo.[EntityId] (Id) Values('32')
						Insert Into dbo.[EntityId] (Id) Values('33')
						Insert Into dbo.[EntityId] (Id) Values('34')
						Insert Into dbo.[EntityId] (Id) Values('35')
						Insert Into dbo.[EntityId] (Id) Values('36')
						Insert Into dbo.[EntityId] (Id) Values('37')
						Insert Into dbo.[EntityId] (Id) Values('38')
						Insert Into dbo.[EntityId] (Id) Values('39')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityName] (Id) Values('2')
");
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[EntityRelationships] ON
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('44','40','24')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('16','41','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('17','42','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('40','43','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('45','44','18')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('19','45','20')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('19','46','39')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('42','47','41')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('21','48','22')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('43','49','24')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('25','50','28')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('25','51','26')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('28','52','29')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('24','53','23')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('10','54','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('13','55','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('14','56','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('15','57','1')
						Insert Into dbo.[EntityRelationships] (ChildEntityId,Id,ParentEntityId) Values('18','58','1')
					SET IDENTITY_INSERT dbo.[EntityRelationships] OFF");
			//No test data for MessageType
			//No test data for MessageType
			//No test data for TypeParameter
			//No test data for TypeParameter
			//No test data for ActionEntityType
				Instance.Database.ExecuteSqlCommand(@"
					SET IDENTITY_INSERT dbo.[ApplicationSetting] ON
						Insert Into dbo.[ApplicationSetting] (AutoRun,Id) Values('1','1')
					SET IDENTITY_INSERT dbo.[ApplicationSetting] OFF");
			//No test data for Command
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
			//No test data for EntityAttribute
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityList] (Id) Values('4')
						Insert Into dbo.[EntityList] (Id) Values('14')
						Insert Into dbo.[EntityList] (Id) Values('15')
						Insert Into dbo.[EntityList] (Id) Values('17')
						Insert Into dbo.[EntityList] (Id) Values('20')
						Insert Into dbo.[EntityList] (Id) Values('24')
");
				Instance.Database.ExecuteSqlCommand(@"
						Insert Into dbo.[EntityView] (Id) Values('4')
						Insert Into dbo.[EntityView] (Id) Values('8')
						Insert Into dbo.[EntityView] (Id) Values('9')
						Insert Into dbo.[EntityView] (Id) Values('10')
						Insert Into dbo.[EntityView] (Id) Values('11')
						Insert Into dbo.[EntityView] (Id) Values('12')
						Insert Into dbo.[EntityView] (Id) Values('13')
						Insert Into dbo.[EntityView] (Id) Values('14')
						Insert Into dbo.[EntityView] (Id) Values('15')
						Insert Into dbo.[EntityView] (Id) Values('17')
						Insert Into dbo.[EntityView] (Id) Values('20')
						Insert Into dbo.[EntityView] (Id) Values('21')
						Insert Into dbo.[EntityView] (Id) Values('22')
						Insert Into dbo.[EntityView] (Id) Values('24')
						Insert Into dbo.[EntityView] (Id) Values('34')
");
			//No test data for Event
			//No test data for Message
			//No test data for MessageType
			//No test data for TypeParameter
			//No test data for ViewModel
		}
               
			
		
	}
}
