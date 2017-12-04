﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using GenSoft.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class SystemProcessStateMap
	{
		public static void Map(EntityTypeBuilder<Entities.SystemProcessState> entityBuilder)
		{
			entityBuilder.ToTable("SystemProcessState", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.StateId).HasColumnName("StateId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.ProcessId).HasColumnName("ProcessId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasOne(p => p.SystemProcessStateInfo).WithOne(p => p.SystemProcessState).HasForeignKey<SystemProcessStateInfo>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.State State).WithMany(p => p.SystemProcessState).HasForeignKey(c => c.StateId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.SystemProcess SystemProcess).WithMany(p => p.SystemProcessState).HasForeignKey(c => c.ProcessId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
