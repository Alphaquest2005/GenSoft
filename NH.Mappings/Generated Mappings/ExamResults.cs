﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>


using EF.Entities;
using FluentNHibernate.Mapping;

namespace NH.Mappings
{
	public class ExamResultsMap: ClassMap<ExamResults>
	{
		public ExamResultsMap()
		{
			
			Id(t => t.Id, "Id");        
			  Table("ExamResults");
			  Schema("Diagnostics");
				Map(t => t.ExamDetailsId).Column("ExamDetailsId").Not.LazyLoad();	
				Map(t => t.PatientResultsId).Column("PatientResultsId").Not.LazyLoad();	
		}
	}
}
