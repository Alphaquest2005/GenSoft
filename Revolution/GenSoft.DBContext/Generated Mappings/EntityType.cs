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
	public class EntityTypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityType> entityBuilder)
		{
			entityBuilder.ToTable("EntityType", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionEntityType).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.CompositeRequest).WithOne(p => p.EntityType).HasForeignKey<CompositeRequest>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.DomainEntityType).WithOne(p => p.EntityType).HasForeignKey<DomainEntityType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.Entity).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.EntityList).WithOne(p => p.EntityType).HasForeignKey<EntityList>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypeAttributes).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.EntityView).WithOne(p => p.EntityType).HasForeignKey<EntityView>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.BaseEntityTypes).WithOne(p => p.BaseEntityType).HasForeignKey(c => c.BaseEntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Type Type).WithOne(p => p.EntityType).HasForeignKey<Type>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
