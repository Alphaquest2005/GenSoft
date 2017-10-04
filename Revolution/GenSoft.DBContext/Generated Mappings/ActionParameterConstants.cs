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
	public class ActionParameterConstantsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionParameterConstants> entityBuilder)
		{
			entityBuilder.ToTable("ActionParameterConstants", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionParameterId).HasColumnName("ActionParameterId").IsRequired();
			entityBuilder.Property(t => t.ActionPropertyId).HasColumnName("ActionPropertyId").IsRequired();
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ActionParameters ActionParameters).WithMany(p => p.ActionParameterConstants).HasForeignKey(c => c.ActionParameterId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ActionProperties ActionProperties).WithMany(p => p.ActionParameterConstants).HasForeignKey(c => c.ActionPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
