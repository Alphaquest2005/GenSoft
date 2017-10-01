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
	public class DomainSystemProcessMap
	{
		public static void Map(EntityTypeBuilder<Entities.DomainSystemProcess> entityBuilder)
		{
			entityBuilder.ToTable("DomainSystemProcess", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DomainProcessId).HasColumnName("DomainProcessId").IsRequired();
			entityBuilder.Property(t => t.SystemProcessId).HasColumnName("SystemProcessId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DomainProcess DomainProcess).WithMany(p => p.DomainSystemProcess).HasForeignKey(c => c.DomainProcessId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.SystemProcess SystemProcess).WithMany(p => p.DomainSystemProcess).HasForeignKey(c => c.SystemProcessId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
