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
	public class DomainProcessMainEntityMap
	{
		public static void Map(EntityTypeBuilder<Entities.DomainProcessMainEntity> entityBuilder)
		{
			entityBuilder.ToTable("DomainProcessMainEntity", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.DomainProcessId).HasColumnName("DomainProcessId").IsRequired();
			entityBuilder.Property(t => t.EntityTypeId).HasColumnName("EntityTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.DomainProcess DomainProcess).WithMany(p => p.DomainProcessMainEntity).HasForeignKey(c => c.DomainProcessId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityType EntityType).WithMany(p => p.DomainProcessMainEntity).HasForeignKey(c => c.EntityTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
