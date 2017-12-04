﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class ViewPropertyValueOptionsMap
	{
		public static void Map(EntityTypeBuilder<Entities.ViewPropertyValueOptions> entityBuilder)
		{
			entityBuilder.ToTable("ViewPropertyValueOptions", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Value).HasColumnName("Value").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.ViewPropertyId).HasColumnName("ViewPropertyId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ConfigurationPropertyPresentation).WithOne(p => p.ViewPropertyValueOptions).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.PropertyValueOption).WithOne(p => p.ViewPropertyValueOptions).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewModelPropertyPresentationType).WithOne(p => p.ViewPropertyValueOptions).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewPropertyTheme).WithOne(p => p.ViewPropertyValueOptions).HasForeignKey(c => c.ValueOptionId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.ViewProperty ViewProperty).WithMany(p => p.ViewPropertyValueOptions).HasForeignKey(c => c.ViewPropertyId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
