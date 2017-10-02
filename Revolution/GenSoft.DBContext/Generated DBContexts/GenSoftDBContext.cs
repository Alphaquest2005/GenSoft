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
		public DbSet<Entities.ActionSet> ActionSet { get; set; }
		public DbSet<Entities.ActionSetActions> ActionSetActions { get; set; }
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
		public DbSet<Entities.DataType> DataType { get; set; }
		public DbSet<Entities.DBType> DBType { get; set; }
		public DbSet<Entities.DomainProcess> DomainProcess { get; set; }
		public DbSet<Entities.DomainProcessMainEntity> DomainProcessMainEntity { get; set; }
		public DbSet<Entities.DomainSystemProcess> DomainSystemProcess { get; set; }
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
		public DbSet<Entities.FunctionParameter> FunctionParameter { get; set; }
		public DbSet<Entities.FunctionParameterConstant> FunctionParameterConstant { get; set; }
		public DbSet<Entities.Functions> Functions { get; set; }
		public DbSet<Entities.FunctionSetFunctions> FunctionSetFunctions { get; set; }
		public DbSet<Entities.FunctionSets> FunctionSets { get; set; }
		public DbSet<Entities.Machine> Machine { get; set; }
		public DbSet<Entities.Ordinality> Ordinality { get; set; }
		public DbSet<Entities.ParentEntity> ParentEntity { get; set; }
		public DbSet<Entities.PresentationPropertyType> PresentationPropertyType { get; set; }
		public DbSet<Entities.PresentationTheme> PresentationTheme { get; set; }
		public DbSet<Entities.Process> Process { get; set; }
		public DbSet<Entities.ProcessPath> ProcessPath { get; set; }
		public DbSet<Entities.ProcessStep> ProcessStep { get; set; }
		public DbSet<Entities.ProcessStepEntity> ProcessStepEntity { get; set; }
		public DbSet<Entities.ProcessStepParentEntity> ProcessStepParentEntity { get; set; }
		public DbSet<Entities.ProcessStepRelationship> ProcessStepRelationship { get; set; }
		public DbSet<Entities.RelationshipType> RelationshipType { get; set; }
		public DbSet<Entities.SourceType> SourceType { get; set; }
		public DbSet<Entities.State> State { get; set; }
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
		}
	
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
				ActionMap.Map(modelBuilder.Entity<Entities.Action>());
				ActionParameterConstantsMap.Map(modelBuilder.Entity<Entities.ActionParameterConstants>());
				ActionParameterEntityTypeAttributesMap.Map(modelBuilder.Entity<Entities.ActionParameterEntityTypeAttributes>());
				ActionParametersMap.Map(modelBuilder.Entity<Entities.ActionParameters>());
				ActionPropertiesMap.Map(modelBuilder.Entity<Entities.ActionProperties>());
				ActionPropertyParameterMap.Map(modelBuilder.Entity<Entities.ActionPropertyParameter>());
				ActionSetMap.Map(modelBuilder.Entity<Entities.ActionSet>());
				ActionSetActionsMap.Map(modelBuilder.Entity<Entities.ActionSetActions>());
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
				DataTypeMap.Map(modelBuilder.Entity<Entities.DataType>());
				DBTypeMap.Map(modelBuilder.Entity<Entities.DBType>());
				DomainProcessMap.Map(modelBuilder.Entity<Entities.DomainProcess>());
				DomainProcessMainEntityMap.Map(modelBuilder.Entity<Entities.DomainProcessMainEntity>());
				DomainSystemProcessMap.Map(modelBuilder.Entity<Entities.DomainSystemProcess>());
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
				FunctionParameterMap.Map(modelBuilder.Entity<Entities.FunctionParameter>());
				FunctionParameterConstantMap.Map(modelBuilder.Entity<Entities.FunctionParameterConstant>());
				FunctionsMap.Map(modelBuilder.Entity<Entities.Functions>());
				FunctionSetFunctionsMap.Map(modelBuilder.Entity<Entities.FunctionSetFunctions>());
				FunctionSetsMap.Map(modelBuilder.Entity<Entities.FunctionSets>());
				MachineMap.Map(modelBuilder.Entity<Entities.Machine>());
				OrdinalityMap.Map(modelBuilder.Entity<Entities.Ordinality>());
				ParentEntityMap.Map(modelBuilder.Entity<Entities.ParentEntity>());
				PresentationPropertyTypeMap.Map(modelBuilder.Entity<Entities.PresentationPropertyType>());
				PresentationThemeMap.Map(modelBuilder.Entity<Entities.PresentationTheme>());
				ProcessMap.Map(modelBuilder.Entity<Entities.Process>());
				ProcessPathMap.Map(modelBuilder.Entity<Entities.ProcessPath>());
				ProcessStepMap.Map(modelBuilder.Entity<Entities.ProcessStep>());
				ProcessStepEntityMap.Map(modelBuilder.Entity<Entities.ProcessStepEntity>());
				ProcessStepParentEntityMap.Map(modelBuilder.Entity<Entities.ProcessStepParentEntity>());
				ProcessStepRelationshipMap.Map(modelBuilder.Entity<Entities.ProcessStepRelationship>());
				RelationshipTypeMap.Map(modelBuilder.Entity<Entities.RelationshipType>());
				SourceTypeMap.Map(modelBuilder.Entity<Entities.SourceType>());
				StateMap.Map(modelBuilder.Entity<Entities.State>());
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
