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
	public class ActionSetActionsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionSetActions> entityBuilder)
		{
			entityBuilder.ToTable("ActionSetActions", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionId).HasColumnName("ActionId").IsRequired();
			entityBuilder.Property(t => t.ActionSetId).HasColumnName("ActionSetId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Action Action).WithMany(p => p.ActionSetActions).HasForeignKey(c => c.ActionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ActionSet ActionSet).WithMany(p => p.ActionSetActions).HasForeignKey(c => c.ActionSetId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
