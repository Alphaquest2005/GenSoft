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
	public class ViewPropertyThemeMap
	{
		public static void Map(EntityTypeBuilder<Entities.ViewPropertyTheme> entityBuilder)
		{
			entityBuilder.ToTable("ViewPropertyTheme", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.PresentationThemeId).HasColumnName("PresentationThemeId").IsRequired();
			entityBuilder.Property(t => t.ViewPropertyPresentationPropertyTypeId).HasColumnName("ViewPropertyPresentationPropertyTypeId").IsRequired();
			entityBuilder.Property(t => t.ValueOptionId).HasColumnName("ValueOptionId").IsRequired();
			entityBuilder.Property(t => t.ViewTypeId).HasColumnName("ViewTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.PresentationTheme PresentationTheme).WithMany(p => p.ViewPropertyTheme).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewPropertyPresentationPropertyType ViewPropertyPresentationPropertyType).WithMany(p => p.ViewPropertyTheme).HasForeignKey(c => c.ViewPropertyPresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewPropertyValueOptions ViewPropertyValueOptions).WithMany(p => p.ViewPropertyTheme).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewType ViewType).WithMany(p => p.ViewPropertyTheme).HasForeignKey(c => c.ViewTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
