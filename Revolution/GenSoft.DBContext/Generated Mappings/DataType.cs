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
	public class DataTypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.DataType> entityBuilder)
		{
			entityBuilder.ToTable("DataType", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.Attributes).WithOne(p => p.DataType).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.FunctionParameter).WithOne(p => p.DataType).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.Functions).WithOne(p => p.DataType).HasForeignKey(c => c.ReturnDataTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.PredicateParameters).WithOne(p => p.DataType).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Type Type).WithOne(p => p.DataType).HasForeignKey<Type>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
