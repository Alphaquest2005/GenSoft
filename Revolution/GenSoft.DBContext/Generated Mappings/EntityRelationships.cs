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
	public class EntityRelationshipsMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityRelationships> entityBuilder)
		{
			entityBuilder.ToTable("EntityRelationships", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId").IsRequired();
			entityBuilder.Property(t => t.ParentEntityId).HasColumnName("ParentEntityId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypeAttributes ChildEntity).WithMany(p => p.EntityRelationships).HasForeignKey(c => c.ChildEntityId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityTypeAttributes ParentEntity).WithMany(p => p.EntityRelationships).HasForeignKey(c => c.ParentEntityId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
