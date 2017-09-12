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
	public class ViewPropertyPresentationPropertyTypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.ViewPropertyPresentationPropertyType> entityBuilder)
		{
			entityBuilder.ToTable("ViewPropertyPresentationPropertyType", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.PresentationPropertyTypeId).HasColumnName("PresentationPropertyTypeId").IsRequired();
			entityBuilder.Property(t => t.ViewPropertyId).HasColumnName("ViewPropertyId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityViewModelPresentationProperties).WithOne(p => p.ViewPropertyPresentationPropertyType).HasForeignKey(c => c.ViewPropertyPresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewModelPropertyPresentationType).WithOne(p => p.ViewPropertyPresentationPropertyType).HasForeignKey(c => c.ViewPropertyPresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewPropertyTheme).WithOne(p => p.ViewPropertyPresentationPropertyType).HasForeignKey(c => c.ViewPropertyPresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.PresentationPropertyType PresentationPropertyType).WithMany(p => p.ViewPropertyPresentationPropertyType).HasForeignKey(c => c.PresentationPropertyTypeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewProperty ViewProperty).WithMany(p => p.ViewPropertyPresentationPropertyType).HasForeignKey(c => c.ViewPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}