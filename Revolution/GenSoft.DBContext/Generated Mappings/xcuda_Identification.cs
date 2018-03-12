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
	public class xcuda_IdentificationMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Identification> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Identification", "dbo");
			entityBuilder.HasKey(t => t.ASYCUDA_Id);
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").IsRequired();
			entityBuilder.Property(t => t.Manifest_reference_number).HasColumnName("Manifest_reference_number").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.xcuda_Assessment).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_Assessment>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.xcuda_Type).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_Type>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.xcuda_receipt).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_receipt>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.xcuda_Registration).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_Registration>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.xcuda_Office_segment).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_Office_segment>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_ASYCUDA xcuda_ASYCUDA).WithOne(p => p.xcuda_Identification).HasForeignKey<xcuda_ASYCUDA>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
