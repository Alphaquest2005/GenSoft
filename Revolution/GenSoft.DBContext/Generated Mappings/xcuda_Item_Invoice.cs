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
	public class xcuda_Item_InvoiceMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Item_Invoice> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Item_Invoice", "dbo");
			entityBuilder.HasKey(t => t.Valuation_item_Id);
			entityBuilder.Property(t => t.Valuation_item_Id).HasColumnName("Valuation_item_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Amount_foreign_currency).HasColumnName("Amount_foreign_currency").IsRequired();
			entityBuilder.Property(t => t.Amount_national_currency).HasColumnName("Amount_national_currency").IsRequired();
			entityBuilder.Property(t => t.Currency_code).HasColumnName("Currency_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Currency_rate).HasColumnName("Currency_rate").IsRequired();
			entityBuilder.Property(t => t.Valuation_item_Id).HasColumnName("Valuation_item_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Valuation_item xcuda_Valuation_item).WithOne(p => p.xcuda_Item_Invoice).HasForeignKey<xcuda_Valuation_item>(c => c.Valuation_item_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
