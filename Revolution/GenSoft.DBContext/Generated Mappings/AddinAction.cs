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
	public class AddinActionMap
	{
		public static void Map(EntityTypeBuilder<Entities.AddinAction> entityBuilder)
		{
			entityBuilder.ToTable("AddinActions", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.AddinId).HasColumnName("AddinId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityTypeAddinActions).WithOne(p => p.AddinAction).HasForeignKey(c => c.AddinActionId).OnDelete(DeleteBehavior.Restrict);//AddinActionId//Id//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Addin Addin).WithMany(p => p.AddinActions).HasForeignKey(c => c.AddinId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}