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
	public class xcuda_Taxation_lineMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Taxation_line> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Taxation_line", "dbo");
			entityBuilder.HasKey(t => t.Taxation_line_Id);
			entityBuilder.Property(t => t.Taxation_line_Id).HasColumnName("Taxation_line_Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Taxation_Id).HasColumnName("Taxation_Id").IsRequired();
			entityBuilder.Property(t => t.Duty_tax_amount).HasColumnName("Duty_tax_amount").IsRequired();
			entityBuilder.Property(t => t.Duty_tax_Base).HasColumnName("Duty_tax_Base").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Duty_tax_code).HasColumnName("Duty_tax_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Duty_tax_MP).HasColumnName("Duty_tax_MP").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Duty_tax_rate).HasColumnName("Duty_tax_rate").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Taxation xcuda_Taxation).WithMany(p => p.xcuda_Taxation_line).HasForeignKey(c => c.Taxation_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
