﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using EF.Entities;
using FluentNHibernate.Mapping;

namespace NH.Mappings
{
	public class ZipCodesMap: ClassMap<ZipCodes>
	{
		public ZipCodesMap()
		{
			
			Id(t => t.Id, "Id");        
			  Table("ZipCodes");
			  Schema("dbo");
				Map(t => t.Name).Column("Name").Not.LazyLoad();	
		}
	}
}
