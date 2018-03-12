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
	public class xcuda_DeclarantMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Declarant> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Declarant", "dbo");
			entityBuilder.HasKey(t => t.ASYCUDA_Id);
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").IsRequired();
			entityBuilder.Property(t => t.Declarant_code).HasColumnName("Declarant_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Declarant_name).HasColumnName("Declarant_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Declarant_name).HasColumnName("Declarant_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Declarant_representative).HasColumnName("Declarant_representative").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Number).HasColumnName("Number").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_ASYCUDA xcuda_ASYCUDA).WithOne(p => p.xcuda_Declarant).HasForeignKey<xcuda_ASYCUDA>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
