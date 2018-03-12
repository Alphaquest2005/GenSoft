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
	public class SuppliersMap
	{
		public static void Map(EntityTypeBuilder<Entities.Suppliers> entityBuilder)
		{
			entityBuilder.ToTable("Suppliers", "dbo");
			entityBuilder.HasKey(t => t.SupplierId);
			entityBuilder.Property(t => t.SupplierId).HasColumnName("SupplierId").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.City).HasColumnName("City").IsRequired().HasMaxLength(19);
			entityBuilder.Property(t => t.Country).HasColumnName("Country").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Street).HasColumnName("Street").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.SupplierCode).HasColumnName("SupplierCode").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.SupplierName).HasColumnName("SupplierName").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.SupplierName).HasColumnName("SupplierName").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.ZipCode).HasColumnName("ZipCode").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
	
		}
	}
}
