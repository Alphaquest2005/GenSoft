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
	public class DefaultApplicationMap
	{
		public static void Map(EntityTypeBuilder<Entities.DefaultApplication> entityBuilder)
		{
			entityBuilder.ToTable("DefaultApplication", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Application Application).WithOne(p => p.DefaultApplication).HasForeignKey<Application>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
