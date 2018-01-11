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
	public partial class Parameters: BaseEntity, IParameters
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual int DataTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<ActionParameters> ActionParameters {get; set;}
				public virtual ICollection<PredicateParameters> PredicateParameters {get; set;}
		
			// ---------Parent Relationships
				public virtual DataType DataType {get; set;}
	

	}
}
