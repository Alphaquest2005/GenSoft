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
	public class DeleteMap
	{
		public static void Map(EntityTypeBuilder<Entities.Delete> entityBuilder)
		{
			entityBuilder.ToTable("Deletes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.DateTimeDeleted).HasColumnName("DateTimeDeleted").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Entity Entity).WithOne(p => p.Delete).HasForeignKey<Entity>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
