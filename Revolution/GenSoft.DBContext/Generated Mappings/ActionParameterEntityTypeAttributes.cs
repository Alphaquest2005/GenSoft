﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ActionParameterEntityTypeAttributesMap
	{
		public static void Map(EntityTypeBuilder<Entities.ActionParameterEntityTypeAttributes> entityBuilder)
		{
			entityBuilder.ToTable("ActionParameterEntityTypeAttributes", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.ActionPropertyParameterId).HasColumnName("ActionPropertyParameterId").IsRequired();
			entityBuilder.Property(t => t.EntityTypeAttributeId).HasColumnName("EntityTypeAttributeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ActionPropertyParameter ActionPropertyParameter).WithMany(p => p.ActionParameterEntityTypeAttributes).HasForeignKey(c => c.ActionPropertyParameterId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.EntityTypeAttributes EntityTypeAttributes).WithMany(p => p.ActionParameterEntityTypeAttributes).HasForeignKey(c => c.EntityTypeAttributeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
