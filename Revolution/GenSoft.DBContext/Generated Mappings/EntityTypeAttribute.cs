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
	public class EntityTypeAttributeMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityTypeAttribute> entityBuilder)
		{
			entityBuilder.ToTable("EntityTypeAttributes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.AttributeId).HasColumnName("AttributeId").IsRequired();
			entityBuilder.Property(t => t.EntityTypeId).HasColumnName("EntityTypeId").IsRequired();
			entityBuilder.Property(t => t.Priority).HasColumnName("Priority").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionParameterEntityTypeAttributes).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);//EntityTypeAttributeId//Id//
				entityBuilder.HasOne(p => p.ActionProperty).WithOne(p => p.EntityTypeAttribute).HasForeignKey<ActionProperty>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//
				entityBuilder.HasOne(p => p.BaseEntityTypeAttribute).WithOne(p => p.Base_EntityTypeAttribute).HasForeignKey<BaseEntityTypeAttribute>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//Base_EntityTypeAttributeId
				entityBuilder.HasMany(x => x.Base_EntityTypeAttributes).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.Base_EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);//Base_EntityTypeAttributeId//Id//EntityTypeAttributeId
				entityBuilder.HasOne(p => p.CalculatedProperty).WithOne(p => p.EntityTypeAttribute).HasForeignKey<CalculatedProperty>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//
				entityBuilder.HasMany(x => x.CalculatedPropertyParameterEntityTypes).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);//EntityTypeAttributeId//Id//
				entityBuilder.HasOne(p => p.EntityId).WithOne(p => p.EntityTypeAttribute).HasForeignKey<EntityId>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//
				entityBuilder.HasOne(p => p.EntityName).WithOne(p => p.EntityTypeAttribute).HasForeignKey<EntityName>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//
				entityBuilder.HasMany(x => x.EntityRelationships).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.ChildEntityId).OnDelete(DeleteBehavior.Restrict);//ChildEntityId//Id//
				entityBuilder.HasOne(p => p.EntityTypeAttributeCach).WithOne(p => p.EntityTypeAttribute).HasForeignKey<EntityTypeAttributeCach>(c => c.Id).OnDelete(DeleteBehavior.Restrict);//Id//Id//
				entityBuilder.HasMany(x => x.EntityTypePresentationProperties).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);//EntityTypeAttributeId//Id//
				entityBuilder.HasMany(x => x.ParentEntities).WithOne(p => p.EntityTypeAttribute).HasForeignKey(c => c.ParentEntityId).OnDelete(DeleteBehavior.Restrict);//ParentEntityId//Id//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Attribute Attribute).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.AttributeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityType EntityType).WithMany(p => p.EntityTypeAttributes).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
