﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ActionReferenceTypesMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionReferenceTypes> entityBuilder)
		{
			entityBuilder.ToTable("ActionReferenceTypes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionId).HasColumnName("ActionId").IsRequired();
			entityBuilder.Property(t => t.ReferenceTypeId).HasColumnName("ReferenceTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Action Action).WithMany(p => p.ActionReferenceTypes).HasForeignKey(c => c.ActionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ReferenceTypes ReferenceTypes).WithMany(p => p.ActionReferenceTypes).HasForeignKey(c => c.ReferenceTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
