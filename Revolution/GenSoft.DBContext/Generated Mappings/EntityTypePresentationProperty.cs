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
	public class EntityTypePresentationPropertyMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityTypePresentationProperty> entityBuilder)
		{
			entityBuilder.ToTable("EntityTypePresentationProperty", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EntityTypeAttributeId).HasColumnName("EntityTypeAttributeId").IsRequired();
			entityBuilder.Property(t => t.PresentationThemeId).HasColumnName("PresentationThemeId").IsRequired();
			entityBuilder.Property(t => t.ValueOptionId).HasColumnName("ValueOptionId").IsRequired();
			entityBuilder.Property(t => t.ViewPropertyPresentationPropertyTypeId).HasColumnName("ViewPropertyPresentationPropertyTypeId").IsRequired();
			entityBuilder.Property(t => t.ViewTypeId).HasColumnName("ViewTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityView).WithOne(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.EntityTypePresentationPropertyId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypeAttributes EntityTypeAttributes).WithMany(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.PresentationTheme PresentationTheme).WithMany(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewPropertyValueOptions ViewPropertyValueOptions).WithMany(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewPropertyPresentationPropertyType ViewPropertyPresentationPropertyType).WithMany(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.ViewPropertyPresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewType ViewType).WithMany(p => p.EntityTypePresentationProperty).HasForeignKey(c => c.ViewTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
