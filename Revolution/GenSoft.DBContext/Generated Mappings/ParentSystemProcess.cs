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
	public class ParentSystemProcessMap
	{
		public static void Map(EntityTypeBuilder<Entities.ParentSystemProcess> entityBuilder)
		{
			entityBuilder.ToTable("ParentSystemProcess", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.ParentProcessId).HasColumnName("ParentProcessId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.SystemProcess ).WithOne(p => p.ParentSystemProcess).HasForeignKey<SystemProcess>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.SystemProcess ParentProcess).WithMany(p => p.ParentSystemProcess).HasForeignKey(c => c.ParentProcessId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}