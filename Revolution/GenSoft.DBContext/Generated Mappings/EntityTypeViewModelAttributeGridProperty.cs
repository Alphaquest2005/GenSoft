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
	public class EntityTypeViewModelAttributeGridPropertyMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityTypeViewModelAttributeGridProperty> entityBuilder)
		{
			entityBuilder.ToTable("EntityTypeViewModelAttributeGridProperty", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EntityTypeViewModelAttributeId).HasColumnName("EntityTypeViewModelAttributeId").IsRequired();
			entityBuilder.Property(t => t.ViewPropertyId).HasColumnName("ViewPropertyId").IsRequired();
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.IsWriteView).HasColumnName("IsWriteView").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypeViewModelAttributes EntityTypeViewModelAttributes).WithMany(p => p.EntityTypeViewModelAttributeGridProperty).HasForeignKey(c => c.EntityTypeViewModelAttributeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewProperty ViewProperty).WithMany(p => p.EntityTypeViewModelAttributeGridProperty).HasForeignKey(c => c.ViewPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
