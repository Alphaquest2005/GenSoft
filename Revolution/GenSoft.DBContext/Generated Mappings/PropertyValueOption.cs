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
	public class PropertyValueOptionMap
	{
		public static void Map(EntityTypeBuilder<Entities.PropertyValueOption> entityBuilder)
		{
			entityBuilder.ToTable("PropertyValueOption", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.ValueOptionId).HasColumnName("ValueOptionId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypePresentationProperty EntityTypePresentationProperty).WithOne(p => p.PropertyValueOption).HasForeignKey<EntityTypePresentationProperty>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ViewPropertyValueOptions ViewPropertyValueOptions).WithMany(p => p.PropertyValueOption).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
