﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DBContext.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using GenSoft.Entities;
using GenSoft.Mappings;
using Microsoft.EntityFrameworkCore;

namespace GenSoft.DBContexts
{
	public partial class GenSoftDBContext:DbContext
	{
		public DbSet<Entities.Action> Action { get; set; }
		public DbSet<Entities.ActionParameterConstants> ActionParameterConstants { get; set; }
		public DbSet<Entities.ActionParameterEntityTypeAttributes> ActionParameterEntityTypeAttributes { get; set; }
		public DbSet<Entities.ActionParameters> ActionParameters { get; set; }
		public DbSet<Entities.ActionProperties> ActionProperties { get; set; }
		public DbSet<Entities.ActionPropertyParameter> ActionPropertyParameter { get; set; }
		public DbSet<Entities.ActionReferenceTypes> ActionReferenceTypes { get; set; }
		public DbSet<Entities.ActionSet> ActionSet { get; set; }
		public DbSet<Entities.ActionSetActions> ActionSetActions { get; set; }
		public DbSet<Entities.ActionTrigger> ActionTrigger { get; set; }
		public DbSet<Entities.Agent> Agent { get; set; }
		public DbSet<Entities.Application> Application { get; set; }
		public DbSet<Entities.ApplicationSetting> ApplicationSetting { get; set; }
		public DbSet<Entities.Attributes> Attributes { get; set; }
		public DbSet<Entities.BaseEntityTypeAttribute> BaseEntityTypeAttribute { get; set; }
		public DbSet<Entities.CalculatedProperties> CalculatedProperties { get; set; }
		public DbSet<Entities.CalculatedPropertyParameterEntityTypes> CalculatedPropertyParameterEntityTypes { get; set; }
		public DbSet<Entities.CalculatedPropertyParameters> CalculatedPropertyParameters { get; set; }
		public DbSet<Entities.Command> Command { get; set; }
		public DbSet<Entities.CommandType> CommandType { get; set; }
		public DbSet<Entities.ComplexEventAction> ComplexEventAction { get; set; }
		public DbSet<Entities.ComplexEventActionConstant> ComplexEventActionConstant { get; set; }
		public DbSet<Entities.ComplexEventActionExpectedEvents> ComplexEventActionExpectedEvents { get; set; }
		public DbSet<Entities.ComplexEventActionProcessActions> ComplexEventActionProcessActions { get; set; }
		public DbSet<Entities.ConfigurationPropertyPresentation> ConfigurationPropertyPresentation { get; set; }
		public DbSet<Entities.DatabaseInfo> DatabaseInfo { get; set; }
		public DbSet<Entities.DataType> DataType { get; set; }
		public DbSet<Entities.DBType> DBType { get; set; }
		public DbSet<Entities.DefaultApplication> DefaultApplication { get; set; }
		public DbSet<Entities.DomainProcess> DomainProcess { get; set; }
		public DbSet<Entities.Entity> Entity { get; set; }
		public DbSet<Entities.EntityAttribute> EntityAttribute { get; set; }
		public DbSet<Entities.EntityAttributeChange> EntityAttributeChange { get; set; }
		public DbSet<Entities.EntityId> EntityId { get; set; }
		public DbSet<Entities.EntityName> EntityName { get; set; }
		public DbSet<Entities.EntityRelationship> EntityRelationship { get; set; }
		public DbSet<Entities.EntityType> EntityType { get; set; }
		public DbSet<Entities.EntityTypeAttributeCache> EntityTypeAttributeCache { get; set; }
		public DbSet<Entities.EntityTypeAttributes> EntityTypeAttributes { get; set; }
		public DbSet<Entities.EntityTypePresentationProperty> EntityTypePresentationProperty { get; set; }
		public DbSet<Entities.EntityTypeViewModelCommand> EntityTypeViewModelCommand { get; set; }
		public DbSet<Entities.EntityView> EntityView { get; set; }
		public DbSet<Entities.EventPredicates> EventPredicates { get; set; }
		public DbSet<Entities.EventType> EventType { get; set; }
		public DbSet<Entities.ExpectedEventConstants> ExpectedEventConstants { get; set; }
		public DbSet<Entities.ExpectedEventPredicateParameters> ExpectedEventPredicateParameters { get; set; }
		public DbSet<Entities.ExpectedEvents> ExpectedEvents { get; set; }
		public DbSet<Entities.ExpectedStateEventInfo> ExpectedStateEventInfo { get; set; }
		public DbSet<Entities.FunctionParameter> FunctionParameter { get; set; }
		public DbSet<Entities.FunctionParameterConstant> FunctionParameterConstant { get; set; }
		public DbSet<Entities.Functions> Functions { get; set; }
		public DbSet<Entities.FunctionSetFunctions> FunctionSetFunctions { get; set; }
		public DbSet<Entities.FunctionSets> FunctionSets { get; set; }
		public DbSet<Entities.Machine> Machine { get; set; }
		public DbSet<Entities.MainEntity> MainEntity { get; set; }
		public DbSet<Entities.Ordinality> Ordinality { get; set; }
		public DbSet<Entities.Parameters> Parameters { get; set; }
		public DbSet<Entities.ParentEntity> ParentEntity { get; set; }
		public DbSet<Entities.ParentSystemProcess> ParentSystemProcess { get; set; }
		public DbSet<Entities.PredicateParameters> PredicateParameters { get; set; }
		public DbSet<Entities.Predicates> Predicates { get; set; }
		public DbSet<Entities.PresentationPropertyType> PresentationPropertyType { get; set; }
		public DbSet<Entities.PresentationTheme> PresentationTheme { get; set; }
		public DbSet<Entities.ProcessAction> ProcessAction { get; set; }
		public DbSet<Entities.ProcessActionStateCommandInfo> ProcessActionStateCommandInfo { get; set; }
		public DbSet<Entities.ProcessPath> ProcessPath { get; set; }
		public DbSet<Entities.ProcessStep> ProcessStep { get; set; }
		public DbSet<Entities.ProcessStepComplexActions> ProcessStepComplexActions { get; set; }
		public DbSet<Entities.ProcessStepParentEntity> ProcessStepParentEntity { get; set; }
		public DbSet<Entities.ProcessStepRelationship> ProcessStepRelationship { get; set; }
		public DbSet<Entities.PropertyValue> PropertyValue { get; set; }
		public DbSet<Entities.PropertyValueOption> PropertyValueOption { get; set; }
		public DbSet<Entities.ReferenceTypeName> ReferenceTypeName { get; set; }
		public DbSet<Entities.ReferenceTypes> ReferenceTypes { get; set; }
		public DbSet<Entities.RelationshipType> RelationshipType { get; set; }
		public DbSet<Entities.SourceType> SourceType { get; set; }
		public DbSet<Entities.State> State { get; set; }
		public DbSet<Entities.StateCommandInfo> StateCommandInfo { get; set; }
		public DbSet<Entities.StateEventInfo> StateEventInfo { get; set; }
		public DbSet<Entities.StateInfo> StateInfo { get; set; }
		public DbSet<Entities.StateInfoNotes> StateInfoNotes { get; set; }
		public DbSet<Entities.SystemProcess> SystemProcess { get; set; }
		public DbSet<Entities.SystemProcessState> SystemProcessState { get; set; }
		public DbSet<Entities.SystemProcessStateInfo> SystemProcessStateInfo { get; set; }
		public DbSet<Entities.Type> Type { get; set; }
		public DbSet<Entities.TypeArguement> TypeArguement { get; set; }
		public DbSet<Entities.User> User { get; set; }
		public DbSet<Entities.ViewModelCommands> ViewModelCommands { get; set; }
		public DbSet<Entities.ViewModelPropertyPresentationType> ViewModelPropertyPresentationType { get; set; }
		public DbSet<Entities.ViewModelTypes> ViewModelTypes { get; set; }
		public DbSet<Entities.ViewProperty> ViewProperty { get; set; }
		public DbSet<Entities.ViewPropertyPresentationPropertyType> ViewPropertyPresentationPropertyType { get; set; }
		public DbSet<Entities.ViewPropertyTheme> ViewPropertyTheme { get; set; }
		public DbSet<Entities.ViewPropertyValueOptions> ViewPropertyValueOptions { get; set; }
		public DbSet<Entities.ViewType> ViewType { get; set; }
	
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(DBContext.Properties.Settings.Default.DbConnectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}
	
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
				ActionMap.Map(modelBuilder.Entity<Entities.Action>());
				ActionParameterConstantsMap.Map(modelBuilder.Entity<Entities.ActionParameterConstants>());
				ActionParameterEntityTypeAttributesMap.Map(modelBuilder.Entity<Entities.ActionParameterEntityTypeAttributes>());
				ActionParametersMap.Map(modelBuilder.Entity<Entities.ActionParameters>());
				ActionPropertiesMap.Map(modelBuilder.Entity<Entities.ActionProperties>());
				ActionPropertyParameterMap.Map(modelBuilder.Entity<Entities.ActionPropertyParameter>());
				ActionReferenceTypesMap.Map(modelBuilder.Entity<Entities.ActionReferenceTypes>());
				ActionSetMap.Map(modelBuilder.Entity<Entities.ActionSet>());
				ActionSetActionsMap.Map(modelBuilder.Entity<Entities.ActionSetActions>());
				ActionTriggerMap.Map(modelBuilder.Entity<Entities.ActionTrigger>());
				AgentMap.Map(modelBuilder.Entity<Entities.Agent>());
				ApplicationMap.Map(modelBuilder.Entity<Entities.Application>());
				ApplicationSettingMap.Map(modelBuilder.Entity<Entities.ApplicationSetting>());
				AttributesMap.Map(modelBuilder.Entity<Entities.Attributes>());
				BaseEntityTypeAttributeMap.Map(modelBuilder.Entity<Entities.BaseEntityTypeAttribute>());
				CalculatedPropertiesMap.Map(modelBuilder.Entity<Entities.CalculatedProperties>());
				CalculatedPropertyParameterEntityTypesMap.Map(modelBuilder.Entity<Entities.CalculatedPropertyParameterEntityTypes>());
				CalculatedPropertyParametersMap.Map(modelBuilder.Entity<Entities.CalculatedPropertyParameters>());
				CommandMap.Map(modelBuilder.Entity<Entities.Command>());
				CommandTypeMap.Map(modelBuilder.Entity<Entities.CommandType>());
				ComplexEventActionMap.Map(modelBuilder.Entity<Entities.ComplexEventAction>());
				ComplexEventActionConstantMap.Map(modelBuilder.Entity<Entities.ComplexEventActionConstant>());
				ComplexEventActionExpectedEventsMap.Map(modelBuilder.Entity<Entities.ComplexEventActionExpectedEvents>());
				ComplexEventActionProcessActionsMap.Map(modelBuilder.Entity<Entities.ComplexEventActionProcessActions>());
				ConfigurationPropertyPresentationMap.Map(modelBuilder.Entity<Entities.ConfigurationPropertyPresentation>());
				DatabaseInfoMap.Map(modelBuilder.Entity<Entities.DatabaseInfo>());
				DataTypeMap.Map(modelBuilder.Entity<Entities.DataType>());
				DBTypeMap.Map(modelBuilder.Entity<Entities.DBType>());
				DefaultApplicationMap.Map(modelBuilder.Entity<Entities.DefaultApplication>());
				DomainProcessMap.Map(modelBuilder.Entity<Entities.DomainProcess>());
				EntityMap.Map(modelBuilder.Entity<Entities.Entity>());
				EntityAttributeMap.Map(modelBuilder.Entity<Entities.EntityAttribute>());
				EntityAttributeChangeMap.Map(modelBuilder.Entity<Entities.EntityAttributeChange>());
				EntityIdMap.Map(modelBuilder.Entity<Entities.EntityId>());
				EntityNameMap.Map(modelBuilder.Entity<Entities.EntityName>());
				EntityRelationshipMap.Map(modelBuilder.Entity<Entities.EntityRelationship>());
				EntityTypeMap.Map(modelBuilder.Entity<Entities.EntityType>());
				EntityTypeAttributeCacheMap.Map(modelBuilder.Entity<Entities.EntityTypeAttributeCache>());
				EntityTypeAttributesMap.Map(modelBuilder.Entity<Entities.EntityTypeAttributes>());
				EntityTypePresentationPropertyMap.Map(modelBuilder.Entity<Entities.EntityTypePresentationProperty>());
				EntityTypeViewModelCommandMap.Map(modelBuilder.Entity<Entities.EntityTypeViewModelCommand>());
				EntityViewMap.Map(modelBuilder.Entity<Entities.EntityView>());
				EventPredicatesMap.Map(modelBuilder.Entity<Entities.EventPredicates>());
				EventTypeMap.Map(modelBuilder.Entity<Entities.EventType>());
				ExpectedEventConstantsMap.Map(modelBuilder.Entity<Entities.ExpectedEventConstants>());
				ExpectedEventPredicateParametersMap.Map(modelBuilder.Entity<Entities.ExpectedEventPredicateParameters>());
				ExpectedEventsMap.Map(modelBuilder.Entity<Entities.ExpectedEvents>());
				ExpectedStateEventInfoMap.Map(modelBuilder.Entity<Entities.ExpectedStateEventInfo>());
				FunctionParameterMap.Map(modelBuilder.Entity<Entities.FunctionParameter>());
				FunctionParameterConstantMap.Map(modelBuilder.Entity<Entities.FunctionParameterConstant>());
				FunctionsMap.Map(modelBuilder.Entity<Entities.Functions>());
				FunctionSetFunctionsMap.Map(modelBuilder.Entity<Entities.FunctionSetFunctions>());
				FunctionSetsMap.Map(modelBuilder.Entity<Entities.FunctionSets>());
				MachineMap.Map(modelBuilder.Entity<Entities.Machine>());
				MainEntityMap.Map(modelBuilder.Entity<Entities.MainEntity>());
				OrdinalityMap.Map(modelBuilder.Entity<Entities.Ordinality>());
				ParametersMap.Map(modelBuilder.Entity<Entities.Parameters>());
				ParentEntityMap.Map(modelBuilder.Entity<Entities.ParentEntity>());
				ParentSystemProcessMap.Map(modelBuilder.Entity<Entities.ParentSystemProcess>());
				PredicateParametersMap.Map(modelBuilder.Entity<Entities.PredicateParameters>());
				PredicatesMap.Map(modelBuilder.Entity<Entities.Predicates>());
				PresentationPropertyTypeMap.Map(modelBuilder.Entity<Entities.PresentationPropertyType>());
				PresentationThemeMap.Map(modelBuilder.Entity<Entities.PresentationTheme>());
				ProcessActionMap.Map(modelBuilder.Entity<Entities.ProcessAction>());
				ProcessActionStateCommandInfoMap.Map(modelBuilder.Entity<Entities.ProcessActionStateCommandInfo>());
				ProcessPathMap.Map(modelBuilder.Entity<Entities.ProcessPath>());
				ProcessStepMap.Map(modelBuilder.Entity<Entities.ProcessStep>());
				ProcessStepComplexActionsMap.Map(modelBuilder.Entity<Entities.ProcessStepComplexActions>());
				ProcessStepParentEntityMap.Map(modelBuilder.Entity<Entities.ProcessStepParentEntity>());
				ProcessStepRelationshipMap.Map(modelBuilder.Entity<Entities.ProcessStepRelationship>());
				PropertyValueMap.Map(modelBuilder.Entity<Entities.PropertyValue>());
				PropertyValueOptionMap.Map(modelBuilder.Entity<Entities.PropertyValueOption>());
				ReferenceTypeNameMap.Map(modelBuilder.Entity<Entities.ReferenceTypeName>());
				ReferenceTypesMap.Map(modelBuilder.Entity<Entities.ReferenceTypes>());
				RelationshipTypeMap.Map(modelBuilder.Entity<Entities.RelationshipType>());
				SourceTypeMap.Map(modelBuilder.Entity<Entities.SourceType>());
				StateMap.Map(modelBuilder.Entity<Entities.State>());
				StateCommandInfoMap.Map(modelBuilder.Entity<Entities.StateCommandInfo>());
				StateEventInfoMap.Map(modelBuilder.Entity<Entities.StateEventInfo>());
				StateInfoMap.Map(modelBuilder.Entity<Entities.StateInfo>());
				StateInfoNotesMap.Map(modelBuilder.Entity<Entities.StateInfoNotes>());
				SystemProcessMap.Map(modelBuilder.Entity<Entities.SystemProcess>());
				SystemProcessStateMap.Map(modelBuilder.Entity<Entities.SystemProcessState>());
				SystemProcessStateInfoMap.Map(modelBuilder.Entity<Entities.SystemProcessStateInfo>());
				TypeMap.Map(modelBuilder.Entity<Entities.Type>());
				TypeArguementMap.Map(modelBuilder.Entity<Entities.TypeArguement>());
				UserMap.Map(modelBuilder.Entity<Entities.User>());
				ViewModelCommandsMap.Map(modelBuilder.Entity<Entities.ViewModelCommands>());
				ViewModelPropertyPresentationTypeMap.Map(modelBuilder.Entity<Entities.ViewModelPropertyPresentationType>());
				ViewModelTypesMap.Map(modelBuilder.Entity<Entities.ViewModelTypes>());
				ViewPropertyMap.Map(modelBuilder.Entity<Entities.ViewProperty>());
				ViewPropertyPresentationPropertyTypeMap.Map(modelBuilder.Entity<Entities.ViewPropertyPresentationPropertyType>());
				ViewPropertyThemeMap.Map(modelBuilder.Entity<Entities.ViewPropertyTheme>());
				ViewPropertyValueOptionsMap.Map(modelBuilder.Entity<Entities.ViewPropertyValueOptions>());
				ViewTypeMap.Map(modelBuilder.Entity<Entities.ViewType>());
			}
	}
}
