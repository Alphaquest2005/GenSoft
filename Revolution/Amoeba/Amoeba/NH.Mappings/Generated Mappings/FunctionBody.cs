﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using EF.Entities;
using FluentNHibernate.Mapping;

namespace NH.Mappings
{
	public class FunctionBodyMap: ClassMap<FunctionBody>
	{
		public FunctionBodyMap()
		{
			
			Id(t => t.Id, "Id");        
			  Table("FunctionBody");
			  Schema("dbo");
				Map(t => t.Body).Column("Body").Not.LazyLoad();	
				Map(t => t.Id).Column("Id").Not.LazyLoad();	
		}
	}
}
