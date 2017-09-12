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
	public class EntityTypeViewModelMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityTypeViewModel> entityBuilder)
		{
			entityBuilder.ToTable("EntityTypeViewModel", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.Priority).HasColumnName("Priority").IsRequired();
			entityBuilder.Property(t => t.ProcessDomainEntityTypeId).HasColumnName("ProcessDomainEntityTypeId").IsRequired();
			entityBuilder.Property(t => t.PropertyName).HasColumnName("PropertyName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.PropertyName).HasColumnName("PropertyName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Symbol).HasColumnName("Symbol").IsRequired().HasMaxLength(3);
			entityBuilder.Property(t => t.ViewModelTypeId).HasColumnName("ViewModelTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityViewModelCommands).WithOne(p => p.EntityTypeViewModel).HasForeignKey(c => c.EntityViewModelId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypeViewModelAttributes).WithOne(p => p.EntityTypeViewModel).HasForeignKey(c => c.EntityTypeViewModelId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ProcessStateDomainEntityTypes ProcessStateDomainEntityTypes).WithMany(p => p.EntityTypeViewModel).HasForeignKey(c => c.ProcessDomainEntityTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewModelTypes ViewModelTypes).WithMany(p => p.EntityTypeViewModel).HasForeignKey(c => c.ViewModelTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
