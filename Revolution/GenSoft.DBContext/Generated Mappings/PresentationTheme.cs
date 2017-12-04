﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class PresentationThemeMap
	{
		public static void Map(EntityTypeBuilder<Entities.PresentationTheme> entityBuilder)
		{
			entityBuilder.ToTable("PresentationTheme", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.ConfigurationPropertyPresentation).WithOne(p => p.PresentationTheme).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.EntityTypePresentationProperty).WithOne(p => p.PresentationTheme).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewPropertyTheme).WithOne(p => p.PresentationTheme).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.ViewModelPropertyPresentationType).WithOne(p => p.PresentationTheme).HasForeignKey(c => c.PresentationThemeId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
	
		}
	}
}
