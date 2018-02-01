﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenSoft.Mappings
{
	public class FunctionParameterMap
	{
		public static void Map(EntityTypeBuilder<Entities.FunctionParameter> entityBuilder)
		{
			entityBuilder.ToTable("FunctionParameter", "dbo");
			entityBuilder.HasKey(t => t.Id);
			entityBuilder.Property(t => t.Id).HasColumnName("Id").UseSqlServerIdentityColumn();	
			entityBuilder.Property(t => t.FunctionId).HasColumnName("FunctionId").IsRequired();
			entityBuilder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
			entityBuilder.Property(t => t.DataTypeId).HasColumnName("DataTypeId").IsRequired();
		//-------------------Navigation Properties -------------------------------//
				entityBuilder.HasMany(x => x.CalculatedPropertyParameters).WithOne(p => p.FunctionParameter).HasForeignKey(c => c.FunctionParameterId).OnDelete(DeleteBehavior.Restrict);
				entityBuilder.HasMany(x => x.FunctionParameterConstant).WithOne(p => p.FunctionParameter).HasForeignKey(c => c.FunctionParameterId).OnDelete(DeleteBehavior.Restrict);
	
				//----------------Parent Properties
				//entityBuilder.HasOne(p => p.Functions Functions).WithMany(p => p.FunctionParameter).HasForeignKey(c => c.FunctionId).OnDelete(DeleteBehavior.Restrict);
				//entityBuilder.HasOne(p => p.DataType DataType).WithMany(p => p.FunctionParameter).HasForeignKey(c => c.DataTypeId).OnDelete(DeleteBehavior.Restrict);
	
		}
	}
}
