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
	public class EntityTypeAttributesMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityTypeAttributes> entityBuilder)
		{
			entityBuilder.ToTable("EntityTypeAttributes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DataTypeId).HasColumnName("DataTypeId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.EntityTypeId).HasColumnName("EntityTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.EntityId).WithOne(p => p.EntityTypeAttributes).HasForeignKey<EntityId>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.EntityName).WithOne(p => p.EntityTypeAttributes).HasForeignKey<EntityName>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DataType DataType).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityType EntityType).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}