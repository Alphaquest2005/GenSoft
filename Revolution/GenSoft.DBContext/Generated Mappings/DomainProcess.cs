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
	public class DomainProcessMap
	{
		public static void Map(EntityTypeBuilder<Entities.DomainProcess> entityBuilder)
		{
			entityBuilder.ToTable("DomainProcess", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.ApplicationId).HasColumnName("ApplicationId").IsRequired();
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.Priority).HasColumnName("Priority").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ProcessStep).WithOne(p => p.DomainProcess).HasForeignKey(c => c.DomainProcessId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Application Application).WithMany(p => p.DomainProcess).HasForeignKey(c => c.ApplicationId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.SystemProcess SystemProcess).WithOne(p => p.DomainProcess).HasForeignKey<SystemProcess>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
