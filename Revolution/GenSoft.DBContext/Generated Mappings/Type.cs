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
	public class TypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.Type> entityBuilder)
		{
			entityBuilder.ToTable("Type", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.DataType).WithOne(p => p.Type).HasForeignKey<DataType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.EntityType).WithOne(p => p.Type).HasForeignKey<EntityType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.MessageType).WithOne(p => p.Type).HasForeignKey<MessageType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.SourceType).WithOne(p => p.Type).HasForeignKey<SourceType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypes).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ParameterTypes).WithOne(p => p.ParameterType).HasForeignKey(c => c.ParameterTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}
