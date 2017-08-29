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
			entityBuilder.Property(t => t.AttributeId).HasColumnName("AttributeId").IsRequired();
			entityBuilder.Property(t => t.EntityTypeId).HasColumnName("EntityTypeId").IsRequired();
			entityBuilder.Property(t => t.Priority).HasColumnName("Priority").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ChildEntitys).WithOne(p => p.ChildEntity).HasForeignKey(c => c.ChildEntityId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ParentEntitys).WithOne(p => p.ParentEntity).HasForeignKey(c => c.ParentEntityId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Attributes Attributes).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.AttributeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityType EntityType).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
