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
	public class CommandTypeMap
	{
		public static void Map(EntityTypeBuilder<Entities.CommandType> entityBuilder)
		{
			entityBuilder.ToTable("CommandTypes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ViewModelCommands).WithOne(p => p.CommandType).HasForeignKey(c => c.CommandTypeId).OnDelete(DeleteBehavior.Restrict);//CommandTypeId//Id//
	
				//----------------Parent Properties
	
		}
	}
}
