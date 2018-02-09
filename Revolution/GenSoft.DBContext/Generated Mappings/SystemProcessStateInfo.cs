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
	public class SystemProcessStateInfoMap
	{
		public static void Map(EntityTypeBuilder<Entities.SystemProcessStateInfo> entityBuilder)
		{
			entityBuilder.ToTable("SystemProcessStateInfo", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.Notes).HasColumnName("Notes").IsRequired().HasMaxLength(Int32.MaxValue);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.SystemProcessState SystemProcessState).WithOne(p => p.SystemProcessStateInfo).HasForeignKey<SystemProcessState>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
