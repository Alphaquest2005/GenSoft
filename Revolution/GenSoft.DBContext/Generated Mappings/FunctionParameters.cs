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
	public class FunctionParametersMap
	{
		public static void Map(EntityTypeBuilder<Entities.FunctionParameters> entityBuilder)
		{
			entityBuilder.ToTable("FunctionParameters", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DataTypeId).HasColumnName("DataTypeId").IsRequired();
			entityBuilder.Property(t => t.FunctionId).HasColumnName("FunctionId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.CalculatedPropertyParameters).WithOne(p => p.FunctionParameters).HasForeignKey(c => c.FunctionParameterId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.FunctionParameterConstants).WithOne(p => p.FunctionParameters).HasForeignKey(c => c.FunctionParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DataType DataType).WithMany(p => p.FunctionParameters).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Functions Functions).WithMany(p => p.FunctionParameters).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}