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
	public class xcuda_Office_segmentMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Office_segment> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Office_segment", "dbo");
			entityBuilder.HasKey(t => t.ASYCUDA_Id);
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").IsRequired();
			entityBuilder.Property(t => t.Customs_clearance_office_code).HasColumnName("Customs_clearance_office_code").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Customs_Clearance_office_name).HasColumnName("Customs_Clearance_office_name").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Customs_Clearance_office_name).HasColumnName("Customs_Clearance_office_name").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Identification xcuda_Identification).WithOne(p => p.xcuda_Office_segment).HasForeignKey<xcuda_Identification>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}