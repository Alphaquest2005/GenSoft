﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using EF.Entities;
using FluentNHibernate.Mapping;

namespace NH.Mappings
{
	public class FunctionReturnTypeMap: ClassMap<FunctionReturnType>
	{
		public FunctionReturnTypeMap()
		{
			
			Id(t => t.Id, "Id");        
			  Table("FunctionReturnType");
			  Schema("dbo");
				Map(t => t.DataTypeId).Column("DataTypeId").Not.LazyLoad();	
				Map(t => t.Id).Column("Id").Not.LazyLoad();	
		}
	}
}
