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
	public class EntityAttributeMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityAttribute> entityBuilder)
		{
			entityBuilder.ToTable("EntityAttribute", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.AttributeId).HasColumnName("AttributeId").IsRequired();
			entityBuilder.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired();
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(255);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.Predecessors).WithOne(p => p.Predecessors).HasForeignKey(c => c.PredecessorId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.Successors).WithOne(p => p.Successors).HasForeignKey(c => c.SuccessorId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Attributes Attributes).WithMany(p => p.EntityAttribute).HasForeignKey(c => c.AttributeId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Entity Entity).WithMany(p => p.EntityAttribute).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
