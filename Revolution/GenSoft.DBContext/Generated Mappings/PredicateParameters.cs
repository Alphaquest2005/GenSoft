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
	public class PredicateParametersMap
	{
		public static void Map(EntityTypeBuilder<Entities.PredicateParameters> entityBuilder)
		{
			entityBuilder.ToTable("PredicateParameters", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.PredicateId).HasColumnName("PredicateId").IsRequired();
			entityBuilder.Property(t => t.ParameterId).HasColumnName("ParameterId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ExpectedEventPredicateParameters).WithOne(p => p.PredicateParameters).HasForeignKey(c => c.PredicateParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Predicates Predicates).WithMany(p => p.PredicateParameters).HasForeignKey(c => c.PredicateId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Parameters Parameters).WithMany(p => p.PredicateParameters).HasForeignKey(c => c.ParameterId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
