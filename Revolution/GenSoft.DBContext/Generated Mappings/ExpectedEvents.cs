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
	public class ExpectedEventsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ExpectedEvents> entityBuilder)
		{
			entityBuilder.ToTable("ExpectedEvents", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EventTypeId).HasColumnName("EventTypeId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ComplexEventActionExpectedEvents).WithOne(p => p.ExpectedEvents).HasForeignKey(c => c.ExpectedEventId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ExpectedEventPredicateParameters).WithOne(p => p.ExpectedEvents).HasForeignKey(c => c.ExpectedEventId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EventType EventType).WithMany(p => p.ExpectedEvents).HasForeignKey(c => c.EventTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}