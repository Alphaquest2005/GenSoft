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
	public class ParentEntityMap
	{
		public static void Map(EntityTypeBuilder<Entities.ParentEntity> entityBuilder)
		{
			entityBuilder.ToTable("ParentEntity", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.ParentEntityId).HasColumnName("ParentEntityId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityRelationship EntityRelationship).WithOne(p => p.ParentEntity).HasForeignKey<EntityRelationship>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityTypeAttributes EntityTypeAttributes).WithMany(p => p.ParentEntity).HasForeignKey(c => c.ParentEntityId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
