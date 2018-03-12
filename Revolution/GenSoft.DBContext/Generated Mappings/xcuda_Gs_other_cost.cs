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
	public class xcuda_Gs_other_costMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Gs_other_cost> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Gs_other_cost", "dbo");
			entityBuilder.HasKey(t => t.Valuation_Id);
			entityBuilder.Property(t => t.Valuation_Id).HasColumnName("Valuation_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Amount_foreign_currency).HasColumnName("Amount_foreign_currency").IsRequired();
			entityBuilder.Property(t => t.Amount_national_currency).HasColumnName("Amount_national_currency").IsRequired();
			entityBuilder.Property(t => t.Currency_name).HasColumnName("Currency_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Currency_name).HasColumnName("Currency_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Currency_rate).HasColumnName("Currency_rate").IsRequired();
			entityBuilder.Property(t => t.Valuation_Id).HasColumnName("Valuation_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Valuation xcuda_Valuation).WithOne(p => p.xcuda_Gs_other_cost).HasForeignKey<xcuda_Valuation>(c => c.Valuation_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
