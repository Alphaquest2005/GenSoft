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
	public partial class ProcessStepComplexAction: BaseEntity, IProcessStepComplexAction
	{
		public virtual int ComplexEventActionId { get; set; }
		public virtual int ProcessStepId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual ComplexEventAction ComplexEventAction {get; set;}
				public virtual ProcessStep ProcessStep {get; set;}
	

	}
}