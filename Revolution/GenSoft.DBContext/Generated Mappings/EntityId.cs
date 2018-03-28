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
	public class EntityIdMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityId> entityBuilder)
		{
			entityBuilder.ToTable("EntityIds", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.IsEntityId).HasColumnName("IsEntityId").IsRequired();
			entityBuilder.Property(t => t.IsComputed).HasColumnName("IsComputed").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypeAttribute EntityTypeAttribute).WithOne(p => p.EntityId).HasForeignKey<EntityTypeAttribute>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
