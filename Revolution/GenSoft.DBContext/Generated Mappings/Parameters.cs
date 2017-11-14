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
	public class ParametersMap
	{
		public static void Map(EntityTypeBuilder<Entities.Parameters> entityBuilder)
		{
			entityBuilder.ToTable("Parameters", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DataTypeId).HasColumnName("DataTypeId").IsRequired();
			entityBuilder.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionParameters).WithOne(p => p.Parameters).HasForeignKey(c => c.ParameterId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.PredicateParameters).WithOne(p => p.Parameters).HasForeignKey(c => c.ParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DataType DataType).WithMany(p => p.Parameters).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
