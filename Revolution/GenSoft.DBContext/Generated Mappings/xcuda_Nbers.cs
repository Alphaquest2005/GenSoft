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
	public class xcuda_NbersMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Nbers> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Nbers", "dbo");
			entityBuilder.HasKey(t => t.ASYCUDA_Id);
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.ASYCUDA_Id).HasColumnName("ASYCUDA_Id").IsRequired();
			entityBuilder.Property(t => t.Number_of_loading_lists).HasColumnName("Number_of_loading_lists").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Total_number_of_items).HasColumnName("Total_number_of_items").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.Total_number_of_packages).HasColumnName("Total_number_of_packages").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Property xcuda_Property).WithOne(p => p.xcuda_Nbers).HasForeignKey<xcuda_Property>(c => c.ASYCUDA_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}