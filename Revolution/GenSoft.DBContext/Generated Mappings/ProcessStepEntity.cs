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
	public class ProcessStepEntityMap
	{
		public static void Map(EntityTypeBuilder<Entities.ProcessStepEntity> entityBuilder)
		{
			entityBuilder.ToTable("ProcessStepEntity", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired();
			entityBuilder.Property(t => t.ProcessStepId).HasColumnName("ProcessStepId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityType EntityType).WithMany(p => p.ProcessStepEntity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ProcessStep ProcessStep).WithMany(p => p.ProcessStepEntity).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
