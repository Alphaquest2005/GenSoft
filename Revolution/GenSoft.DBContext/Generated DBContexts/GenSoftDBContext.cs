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
		public DbSet<Entities.Agent> Agent { get; set; }
		public DbSet<Entities.ApplicationSetting> ApplicationSetting { get; set; }
		public DbSet<Entities.Command> Command { get; set; }
		public DbSet<Entities.Entity> Entity { get; set; }
		public DbSet<Entities.EntityAttribute> EntityAttribute { get; set; }
		public DbSet<Entities.EntityType> EntityType { get; set; }
		public DbSet<Entities.Event> Event { get; set; }
		public DbSet<Entities.Machine> Machine { get; set; }
		public DbSet<Entities.Message> Message { get; set; }
		public DbSet<Entities.MessageSource> MessageSource { get; set; }
		public DbSet<Entities.Process> Process { get; set; }
		public DbSet<Entities.ProcessComplexState> ProcessComplexState { get; set; }
		public DbSet<Entities.ProcessComplexStateExpectedProcessState> ProcessComplexStateExpectedProcessState { get; set; }
		public DbSet<Entities.ProcessState> ProcessState { get; set; }
		public DbSet<Entities.ProcessStateInfo> ProcessStateInfo { get; set; }
		public DbSet<Entities.ProcessStateTrigger> ProcessStateTrigger { get; set; }
		public DbSet<Entities.SourceType> SourceType { get; set; }
		public DbSet<Entities.State> State { get; set; }
		public DbSet<Entities.StateAction> StateAction { get; set; }
		public DbSet<Entities.StateActionExpectedProcessState> StateActionExpectedProcessState { get; set; }
		public DbSet<Entities.Type> Type { get; set; }
		public DbSet<Entities.TypeParameter> TypeParameter { get; set; }
		public DbSet<Entities.User> User { get; set; }
		public DbSet<Entities.ViewModel> ViewModel { get; set; }
	
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(DBContext.Properties.Settings.Default.DbConnectionString);
		}
	
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
				ActionMap.Map(modelBuilder.Entity<Entities.Action>());
				AgentMap.Map(modelBuilder.Entity<Entities.Agent>());
				ApplicationSettingMap.Map(modelBuilder.Entity<Entities.ApplicationSetting>());
				CommandMap.Map(modelBuilder.Entity<Entities.Command>());
				EntityMap.Map(modelBuilder.Entity<Entities.Entity>());
				EntityAttributeMap.Map(modelBuilder.Entity<Entities.EntityAttribute>());
				EntityTypeMap.Map(modelBuilder.Entity<Entities.EntityType>());
				EventMap.Map(modelBuilder.Entity<Entities.Event>());
				MachineMap.Map(modelBuilder.Entity<Entities.Machine>());
				MessageMap.Map(modelBuilder.Entity<Entities.Message>());
				MessageSourceMap.Map(modelBuilder.Entity<Entities.MessageSource>());
				ProcessMap.Map(modelBuilder.Entity<Entities.Process>());
				ProcessComplexStateMap.Map(modelBuilder.Entity<Entities.ProcessComplexState>());
				ProcessComplexStateExpectedProcessStateMap.Map(modelBuilder.Entity<Entities.ProcessComplexStateExpectedProcessState>());
				ProcessStateMap.Map(modelBuilder.Entity<Entities.ProcessState>());
				ProcessStateInfoMap.Map(modelBuilder.Entity<Entities.ProcessStateInfo>());
				ProcessStateTriggerMap.Map(modelBuilder.Entity<Entities.ProcessStateTrigger>());
				SourceTypeMap.Map(modelBuilder.Entity<Entities.SourceType>());
				StateMap.Map(modelBuilder.Entity<Entities.State>());
				StateActionMap.Map(modelBuilder.Entity<Entities.StateAction>());
				StateActionExpectedProcessStateMap.Map(modelBuilder.Entity<Entities.StateActionExpectedProcessState>());
				TypeMap.Map(modelBuilder.Entity<Entities.Type>());
				TypeParameterMap.Map(modelBuilder.Entity<Entities.TypeParameter>());
				UserMap.Map(modelBuilder.Entity<Entities.User>());
				ViewModelMap.Map(modelBuilder.Entity<Entities.ViewModel>());
			}
	}
}
