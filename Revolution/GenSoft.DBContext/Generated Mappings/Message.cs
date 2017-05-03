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
	public class MessageMap
	{
		public static void Map(EntityTypeBuilder<Entities.Message> entityBuilder)
		{
			entityBuilder.ToTable("Message", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.MessageSourceId).HasColumnName("MessageSourceId").IsRequired();
			entityBuilder.Property(t => t.MachineId).HasColumnName("MachineId").IsRequired();
			entityBuilder.Property(t => t.ProcessId).HasColumnName("ProcessId").IsRequired();
			entityBuilder.Property(t => t.EntryDateTime).HasColumnName("EntryDateTime").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.Command).WithOne(p => p.Message).HasForeignKey<Command>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasOne(p => p.Event).WithOne(p => p.Message).HasForeignKey<Event>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.MessageSource MessageSource).WithMany(p => p.Message).HasForeignKey(c => c.MessageSourceId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Machine Machine).WithMany(p => p.Message).HasForeignKey(c => c.MachineId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Process Process).WithMany(p => p.Message).HasForeignKey(c => c.ProcessId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
