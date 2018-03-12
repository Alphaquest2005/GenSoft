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
	public class xcuda_HScodeMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_HScode> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_HScode", "dbo");
			entityBuilder.HasKey(t => t.Item_Id);
			entityBuilder.Property(t => t.Item_Id).HasColumnName("Item_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Commodity_code).HasColumnName("Commodity_code").IsRequired().HasMaxLength(8);
			entityBuilder.Property(t => t.Item_Id).HasColumnName("Item_Id").IsRequired();
			entityBuilder.Property(t => t.Precision_1).HasColumnName("Precision_1").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Precision_4).HasColumnName("Precision_4").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.xcuda_Inventory_Item).WithOne(p => p.xcuda_HScode).HasForeignKey<xcuda_Inventory_Item>(c => c.Item_Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Tarification xcuda_Tarification).WithOne(p => p.xcuda_HScode).HasForeignKey<xcuda_Tarification>(c => c.Item_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}