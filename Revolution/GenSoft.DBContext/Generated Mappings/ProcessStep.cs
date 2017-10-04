﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ProcessStepMap
	{
		public static void Map(EntityTypeBuilder<Entities.ProcessStep> entityBuilder)
		{
			entityBuilder.ToTable("ProcessStep", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DomainProcessId).HasColumnName("DomainProcessId").IsRequired();
			entityBuilder.Property(t => t.Descripton).HasColumnName("Descripton").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.Entity).HasColumnName("Entity").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Notes).HasColumnName("Notes").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Symbol).HasColumnName("Symbol").IsRequired().HasMaxLength(10);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityView).WithOne(p => p.ProcessStep).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.NextProcessSteps).WithOne(p => p.NextProcessSteps).HasForeignKey(c => c.NextProcessStepId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ProcessSteps).WithOne(p => p.ProcessSteps).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ProcessStepEntity).WithOne(p => p.ProcessStep).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ProcessStepRelationship).WithOne(p => p.ProcessStep).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DomainProcess DomainProcess).WithMany(p => p.ProcessStep).HasForeignKey(c => c.DomainProcessId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
