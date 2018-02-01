﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ViewModelCommandsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ViewModelCommands> entityBuilder)
		{
			entityBuilder.ToTable("ViewModelCommands", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.CommandTypeId).HasColumnName("CommandTypeId").IsRequired();
			entityBuilder.Property(t => t.RequireAllFields).HasColumnName("RequireAllFields").IsRequired();
			entityBuilder.Property(t => t.ExistingEntities).HasColumnName("ExistingEntities").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.EntityTypeViewModelCommand).WithOne(p => p.ViewModelCommands).HasForeignKey(c => c.ViewModelCommandId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.CommandType CommandType).WithMany(p => p.ViewModelCommands).HasForeignKey(c => c.CommandTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
