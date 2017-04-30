﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Mappings
{
	public class PresentationPropertiesMap
	{
		public static void Map(EntityTypeBuilder<PresentationProperties> entityBuilder)
		{
			entityBuilder.ToTable("PresentationProperties", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.EntityPropertyId).HasColumnName("EntityPropertyId").IsRequired();
			entityBuilder.Property(t => t.DisplayName).HasColumnName("DisplayName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.DisplayName).HasColumnName("DisplayName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.ToolTip).HasColumnName("ToolTip").IsRequired().HasMaxLength(255);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityProperties).WithMany(p => p.PresentationProperties).HasForeignKey(c => c.EntityPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
