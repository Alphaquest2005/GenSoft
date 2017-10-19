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
	public class ComplexActionExpectedEventActionParameterMap
	{
		public static void Map(EntityTypeBuilder<Entities.ComplexActionExpectedEventActionParameter> entityBuilder)
		{
			entityBuilder.ToTable("ComplexActionExpectedEventActionParameter", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionParameterId).HasColumnName("ActionParameterId").IsRequired();
			entityBuilder.Property(t => t.ComplexActionExpectedEventId).HasColumnName("ComplexActionExpectedEventId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.ComplexEventActionConstant).WithOne(p => p.ComplexActionExpectedEventActionParameter).HasForeignKey<ComplexEventActionConstant>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ActionParameters ActionParameters).WithMany(p => p.ComplexActionExpectedEventActionParameter).HasForeignKey(c => c.ActionParameterId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ComplexEventActionExpectedEvents ComplexEventActionExpectedEvents).WithMany(p => p.ComplexActionExpectedEventActionParameter).HasForeignKey(c => c.ComplexActionExpectedEventId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
