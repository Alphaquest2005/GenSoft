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
	public class xcuda_ItemEntryDataDetailsMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_ItemEntryDataDetails> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_ItemEntryDataDetails", "dbo");
			entityBuilder.HasKey(t => t.ItemEntryDataDetailId);
			entityBuilder.Property(t => t.ItemEntryDataDetailId).HasColumnName("ItemEntryDataDetailId").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EntryDataDetailsId).HasColumnName("EntryDataDetailsId").IsRequired();
			entityBuilder.Property(t => t.Item_Id).HasColumnName("Item_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntryDataDetails EntryDataDetails).WithMany(p => p.xcuda_ItemEntryDataDetails).HasForeignKey(c => c.EntryDataDetailsId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.xcuda_Item xcuda_Item).WithMany(p => p.xcuda_ItemEntryDataDetails).HasForeignKey(c => c.Item_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}