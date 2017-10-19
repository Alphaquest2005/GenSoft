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
	public class EventTypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.EventType> entityBuilder)
		{
			entityBuilder.ToTable("EventType", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ComplexEventActionProcessActions).WithOne(p => p.EventType).HasForeignKey(c => c.ExpectedEventTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EventPredicates).WithOne(p => p.EventType).HasForeignKey(c => c.EventTypeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ExpectedEvents).WithOne(p => p.EventType).HasForeignKey(c => c.EventTypeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Type Type).WithOne(p => p.EventType).HasForeignKey<Type>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
