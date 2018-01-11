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
	public class CommandMap
	{
		public static void Map(EntityTypeBuilder<Entities.Command> entityBuilder)
		{
			entityBuilder.ToTable("Command", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.StateActionId).HasColumnName("StateActionId").IsRequired();
			entityBuilder.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired();
			entityBuilder.Property(t => t.CommandTypeId).HasColumnName("CommandTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Entity Entity).WithMany(p => p.Command).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
