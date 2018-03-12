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
	public class EntryDataDetailsMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntryDataDetails> entityBuilder)
		{
			entityBuilder.ToTable("EntryDataDetails", "dbo");
			entityBuilder.HasKey(t => t.EntryDataDetailsId);
			entityBuilder.Property(t => t.EntryDataDetailsId).HasColumnName("EntryDataDetailsId").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Cost).HasColumnName("Cost").IsRequired();
			entityBuilder.Property(t => t.DoNotAllocate).HasColumnName("DoNotAllocate").IsRequired();
			entityBuilder.Property(t => t.Freight).HasColumnName("Freight").IsRequired();
			entityBuilder.Property(t => t.InternalFreight).HasColumnName("InternalFreight").IsRequired();
			entityBuilder.Property(t => t.ItemDescription).HasColumnName("ItemDescription").IsRequired().HasMaxLength(Int32.MaxValue);
			entityBuilder.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();
			entityBuilder.Property(t => t.QtyAllocated).HasColumnName("QtyAllocated").IsRequired();
			entityBuilder.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();
			entityBuilder.Property(t => t.Units).HasColumnName("Units").IsRequired().HasMaxLength(15);
			entityBuilder.Property(t => t.UnitWeight).HasColumnName("UnitWeight").IsRequired();
			entityBuilder.Property(t => t.Weight).HasColumnName("Weight").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.xcuda_ItemEntryDataDetails).WithOne(p => p.EntryDataDetails).HasForeignKey(c => c.EntryDataDetailsId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasMany(x => x.EntryData).WithOne(p => p.EntryDataDetails).HasForeignKey<EntryData>(c => c.EntryDataId);
				//entityBuilder.HasMany(x => x.InventoryItems).WithOne(p => p.EntryDataDetails).HasForeignKey<InventoryItems>(c => c.ItemNumber);
	
		}
	}
}
