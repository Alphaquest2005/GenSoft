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
	public class ActionPropertyParameterMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionPropertyParameter> entityBuilder)
		{
			entityBuilder.ToTable("ActionPropertyParameter", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionParameterId).HasColumnName("ActionParameterId").IsRequired();
			entityBuilder.Property(t => t.ActionPropertyId).HasColumnName("ActionPropertyId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionParameterEntityTypeAttributes).WithOne(p => p.ActionPropertyParameter).HasForeignKey(c => c.ActionPropertyParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ActionParameters ActionParameters).WithMany(p => p.ActionPropertyParameter).HasForeignKey(c => c.ActionParameterId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ActionProperties ActionProperties).WithMany(p => p.ActionPropertyParameter).HasForeignKey(c => c.ActionPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
