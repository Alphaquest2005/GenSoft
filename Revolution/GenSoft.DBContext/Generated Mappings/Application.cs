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
	public class ApplicationMap
	{
		public static void Map(EntityTypeBuilder<Entities.Application> entityBuilder)
		{
			entityBuilder.ToTable("Application", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.DomainProcess).WithOne(p => p.Application).HasForeignKey(c => c.ApplicationId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.DefaultApplication).WithOne(p => p.Application).HasForeignKey<DefaultApplication>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ApplicationSetting).WithOne(p => p.Application).HasForeignKey(c => c.ApplicationId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityType).WithOne(p => p.Application).HasForeignKey(c => c.ApplicationId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.DatabaseInfo).WithOne(p => p.Application).HasForeignKey<DatabaseInfo>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}
