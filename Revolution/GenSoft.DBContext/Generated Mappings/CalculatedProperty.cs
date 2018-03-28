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
	public class CalculatedPropertyMap
	{
		public static void Map(EntityTypeBuilder<Entities.CalculatedProperty> entityBuilder)
		{
			entityBuilder.ToTable("CalculatedProperties", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.FunctionSetId).HasColumnName("FunctionSetId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.CalculatedPropertyParameters).WithOne(p => p.CalculatedProperty).HasForeignKey(c => c.CalculatedPropertyId).OnDelete(DeleteBehavior.Restrict);//CalculatedPropertyId//Id//
				entityBuilder.HasMany(x => x.FunctionParameterConstants).WithOne(p => p.CalculatedProperty).HasForeignKey(c => c.CalculatedPropertyId).OnDelete(DeleteBehavior.Restrict);//CalculatedPropertyId//Id//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypeAttribute EntityTypeAttribute).WithOne(p => p.CalculatedProperty).HasForeignKey<EntityTypeAttribute>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.FunctionSet FunctionSet).WithMany(p => p.CalculatedProperties).HasForeignKey(c => c.FunctionSetId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
