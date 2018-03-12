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
	public class xcuda_Supplementary_unitMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Supplementary_unit> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Supplementary_unit", "dbo");
			entityBuilder.HasKey(t => t.Supplementary_unit_Id);
			entityBuilder.Property(t => t.Supplementary_unit_Id).HasColumnName("Supplementary_unit_Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.IsFirstRow).HasColumnName("IsFirstRow").IsRequired();
			entityBuilder.Property(t => t.Suppplementary_unit_code).HasColumnName("Suppplementary_unit_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppplementary_unit_name).HasColumnName("Suppplementary_unit_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppplementary_unit_name).HasColumnName("Suppplementary_unit_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppplementary_unit_quantity).HasColumnName("Suppplementary_unit_quantity").IsRequired();
			entityBuilder.Property(t => t.Tarification_Id).HasColumnName("Tarification_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Tarification xcuda_Tarification).WithMany(p => p.xcuda_Supplementary_unit).HasForeignKey(c => c.Tarification_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
