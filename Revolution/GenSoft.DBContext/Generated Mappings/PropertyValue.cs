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
	public class PropertyValueMap
	{
		public static void Map(EntityTypeBuilder<Entities.PropertyValue> entityBuilder)
		{
			entityBuilder.ToTable("PropertyValue", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypePresentationProperty EntityTypePresentationProperty).WithOne(p => p.PropertyValue).HasForeignKey<EntityTypePresentationProperty>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
