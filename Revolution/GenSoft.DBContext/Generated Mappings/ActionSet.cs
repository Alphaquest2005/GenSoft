﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ActionSetMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionSet> entityBuilder)
		{
			entityBuilder.ToTable("ActionSet", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionProperties).WithOne(p => p.ActionSet).HasForeignKey(c => c.ActionSetId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ActionSetActions).WithOne(p => p.ActionSet).HasForeignKey(c => c.ActionSetId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}
