﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using Common.DataEntites;
using GenSoft.Entities;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class ExpectedEventPredicateParameters: BaseEntity, IExpectedEventPredicateParameters
	{
		public virtual int EventPredicateId { get; set; }
		public virtual int ExpectedEventId { get; set; }
		public virtual int PredicateParameterId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ExpectedEventConstants ExpectedEventConstants {get; set;}
		
			// ---------Parent Relationships
				public virtual EventPredicates EventPredicates {get; set;}
				public virtual ExpectedEvents ExpectedEvents {get; set;}
				public virtual PredicateParameters PredicateParameters {get; set;}
	

	}
}