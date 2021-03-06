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
	public class xcuda_TaxationMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Taxation> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Taxation", "dbo");
			entityBuilder.HasKey(t => t.Taxation_Id);
			entityBuilder.Property(t => t.Taxation_Id).HasColumnName("Taxation_Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Item_Id).HasColumnName("Item_Id").IsRequired();
			entityBuilder.Property(t => t.Counter_of_normal_mode_of_payment).HasColumnName("Counter_of_normal_mode_of_payment").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Displayed_item_taxes_amount).HasColumnName("Displayed_item_taxes_amount").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Item_taxes_amount).HasColumnName("Item_taxes_amount").IsRequired();
			entityBuilder.Property(t => t.Item_taxes_guaranted_amount).HasColumnName("Item_taxes_guaranted_amount").IsRequired();
			entityBuilder.Property(t => t.Item_taxes_mode_of_payment).HasColumnName("Item_taxes_mode_of_payment").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.xcuda_Taxation_line).WithOne(p => p.xcuda_Taxation).HasForeignKey(c => c.Taxation_Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Item xcuda_Item).WithMany(p => p.xcuda_Taxation).HasForeignKey(c => c.Item_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
