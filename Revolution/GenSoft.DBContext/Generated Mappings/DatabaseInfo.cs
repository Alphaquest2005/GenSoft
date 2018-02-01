﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class DatabaseInfoMap
	{
		public static void Map(EntityTypeBuilder<Entities.DatabaseInfo> entityBuilder)
		{
			entityBuilder.ToTable("DatabaseInfo", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").ValueGeneratedNever();	
			entityBuilder.Property(t => t.Id).HasColumnName("Id").IsRequired();
			entityBuilder.Property(t => t.DBName).HasColumnName("DBName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.DBName).HasColumnName("DBName").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.IsRealDatabase).HasColumnName("IsRealDatabase").IsRequired();
		//-------------------Navigation Properties -------------------------------//
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Application Application).WithOne(p => p.DatabaseInfo).HasForeignKey<Application>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
