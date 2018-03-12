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
	public class xcuda_Suppliers_documentsMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Suppliers_documents> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Suppliers_documents", "dbo");
			entityBuilder.HasKey(t => t.Suppliers_documents_Id);
			entityBuilder.Property(t => t.Suppliers_documents_Id).HasColumnName("Suppliers_documents_Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").IsRequired();
			entityBuilder.Property(t => t.Suppliers_document_city).HasColumnName("Suppliers_document_city").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_code).HasColumnName("Suppliers_document_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_country).HasColumnName("Suppliers_document_country").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_date).HasColumnName("Suppliers_document_date").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_fax).HasColumnName("Suppliers_document_fax").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_invoice_amt).HasColumnName("Suppliers_document_invoice_amt").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_invoice_nbr).HasColumnName("Suppliers_document_invoice_nbr").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_itmlink).HasColumnName("Suppliers_document_itmlink").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_name).HasColumnName("Suppliers_document_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_name).HasColumnName("Suppliers_document_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_street).HasColumnName("Suppliers_document_street").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_telephone).HasColumnName("Suppliers_document_telephone").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_type_code).HasColumnName("Suppliers_document_type_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Suppliers_document_zip_code).HasColumnName("Suppliers_document_zip_code").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_ASYCUDA xcuda_ASYCUDA).WithMany(p => p.xcuda_Suppliers_documents).HasForeignKey(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}