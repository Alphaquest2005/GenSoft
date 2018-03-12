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
	public class TariffSupUnitLkpsMap
	{
		public static void Map(EntityTypeBuilder<Entities.TariffSupUnitLkps> entityBuilder)
		{
			entityBuilder.ToTable("TariffSupUnitLkps", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.SuppQty).HasColumnName("SuppQty").IsRequired();
			entityBuilder.Property(t => t.SuppUnitCode2).HasColumnName("SuppUnitCode2").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.SuppUnitName2).HasColumnName("SuppUnitName2").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.TariffCategoryCodeSuppUnit).WithOne(p => p.TariffSupUnitLkps).HasForeignKey(c => c.TariffSupUnitId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}