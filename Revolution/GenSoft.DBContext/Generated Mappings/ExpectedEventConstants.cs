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
	public class ExpectedEventConstantsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ExpectedEventConstants> entityBuilder)
		{
			entityBuilder.ToTable("ExpectedEventConstants", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ExpectedEventPredicateParameters ExpectedEventPredicateParameters).WithOne(p => p.ExpectedEventConstants).HasForeignKey<ExpectedEventPredicateParameters>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
