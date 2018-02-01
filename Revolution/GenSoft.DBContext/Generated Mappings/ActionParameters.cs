﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ActionParametersMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionParameters> entityBuilder)
		{
			entityBuilder.ToTable("ActionParameters", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionId).HasColumnName("ActionId").IsRequired();
			entityBuilder.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(255);
			entityBuilder.Property(t => t.ParameterId).HasColumnName("ParameterId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ActionPropertyParameter).WithOne(p => p.ActionParameters).HasForeignKey(c => c.ActionParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Action Action).WithMany(p => p.ActionParameters).HasForeignKey(c => c.ActionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.Parameters Parameters).WithMany(p => p.ActionParameters).HasForeignKey(c => c.ParameterId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
