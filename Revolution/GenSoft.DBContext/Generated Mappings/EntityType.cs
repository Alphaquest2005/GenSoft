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
			entityBuilder.Property(t => t.ApplicationId).HasColumnName("ApplicationId").IsRequired();
			entityBuilder.Property(t => t.EntitySet).HasColumnName("EntitySet").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.DBType).WithOne(p => p.EntityType).HasForeignKey<DBType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.Entity).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.MainEntity).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypeViewModelCommand).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.ParentEntityType).WithOne(p => p.ParentEntityTypes).HasForeignKey<ParentEntityType>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ParentEntityTypes).WithOne(p => p.EntityType).HasForeignKey(c => c.ParentEntityTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypeAttributes).WithOne(p => p.EntityType).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Application Application).WithMany(p => p.EntityType).HasForeignKey(c => c.ApplicationId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Type Type).WithOne(p => p.EntityType).HasForeignKey<Type>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
