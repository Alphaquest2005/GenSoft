﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Mappings
{
	public class FunctionsMap
	{
		public static void Map(EntityTypeBuilder<Functions> entityBuilder)
		{
			entityBuilder.ToTable("Functions", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityViewPropertyFunction).WithOne(p => p.Functions).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.FunctionBody).WithOne(p => p.Functions).HasForeignKey<FunctionBody>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.FunctionParameters).WithOne(p => p.Functions).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.FunctionReturnType).WithOne(p => p.Functions).HasForeignKey<FunctionReturnType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}
