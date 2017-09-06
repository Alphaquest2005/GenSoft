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
	public class FunctionsMap
	{
		public static void Map(EntityTypeBuilder<Entities.Functions> entityBuilder)
		{
			entityBuilder.ToTable("Functions", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ReturnDataTypeId).HasColumnName("ReturnDataTypeId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Body).HasColumnName("Body").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.FunctionParameters).WithOne(p => p.Functions).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.FunctionSetFunctions).WithOne(p => p.Functions).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DataType DataType).WithMany(p => p.Functions).HasForeignKey(c => c.ReturnDataTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
