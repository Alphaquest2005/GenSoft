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
	public partial class ProcessComplexStateExpectedProcessState: BaseEntity, IProcessComplexStateExpectedProcessState
	{
		public virtual int ComplexStateId { get; set; }
		public virtual int ProcessStateId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual ProcessComplexState ProcessComplexState {get; set;}
				public virtual ProcessState ProcessState {get; set;}
	

	}
}
