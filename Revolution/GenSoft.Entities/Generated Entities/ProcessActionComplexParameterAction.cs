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
	public partial class ProcessActionComplexParameterAction: BaseEntity, IProcessActionComplexParameterAction
	{
		public virtual string Body { get; set; }
		public virtual string ParameterName { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<ProcessActionComplexParameterReferenceTypes> ProcessActionComplexParameterReferenceTypes {get; set;}
		
			// ---------Parent Relationships
				public virtual ProcessAction ProcessAction {get; set;}
	

	}
}
