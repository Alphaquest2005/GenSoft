﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class EntityViewMap
	{
		public static void Map(EntityTypeBuilder<Entities.EntityView> entityBuilder)
		{
			entityBuilder.ToTable("EntityView", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.EntityTypePresentationPropertyId).HasColumnName("EntityTypePresentationPropertyId").IsRequired();
			entityBuilder.Property(t => t.ProcessStepId).HasColumnName("ProcessStepId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.EntityTypePresentationProperty EntityTypePresentationProperty).WithMany(p => p.EntityView).HasForeignKey(c => c.EntityTypePresentationPropertyId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.ProcessStep ProcessStep).WithMany(p => p.EntityView).HasForeignKey(c => c.ProcessStepId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
