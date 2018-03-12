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
	public class xcuda_Delivery_termsMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Delivery_terms> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Delivery_terms", "dbo");
			entityBuilder.HasKey(t => t.Delivery_terms_Id);
			entityBuilder.Property(t => t.Delivery_terms_Id).HasColumnName("Delivery_terms_Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Place).HasColumnName("Place").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Situation).HasColumnName("Situation").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.Transport_Id).HasColumnName("Transport_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Transport xcuda_Transport).WithMany(p => p.xcuda_Delivery_terms).HasForeignKey(c => c.Transport_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
