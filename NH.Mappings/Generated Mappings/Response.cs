﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using EF.Entities;
using FluentNHibernate.Mapping;

namespace NH.Mappings
{
	public class ResponseMap: ClassMap<Response>
	{
		public ResponseMap()
		{
			
			Id(t => t.Id, "Id");        
			  Table("Response");
			  Schema("Interview");
				Map(t => t.PatientResponseId).Column("PatientResponseId").Not.LazyLoad();	
				Map(t => t.ResponseOptionId).Column("ResponseOptionId").Not.LazyLoad();	
				Map(t => t.Value).Column("Value").Not.LazyLoad();	
		}
	}
}
