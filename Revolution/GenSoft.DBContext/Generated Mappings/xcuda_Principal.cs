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
	public class xcuda_PrincipalMap
	{
		public static void Map(EntityTypeBuilder<Entities.xcuda_Principal> entityBuilder)
		{
			entityBuilder.ToTable("xcuda_Principal", "dbo");
			entityBuilder.HasKey(t => t.Principal_Id);
			entityBuilder.Property(t => t.Principal_Id).HasColumnName("Principal_Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Transit_Id).HasColumnName("Transit_Id").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.xcuda_Transit xcuda_Transit).WithMany(p => p.xcuda_Principal).HasForeignKey(c => c.Transit_Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}