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
	public partial class FunctionSetFunctions: BaseEntity, IFunctionSetFunctions
	{
		public virtual int FunctionId { get; set; }
		public virtual int FunctionSetId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Functions Functions {get; set;}
				public virtual FunctionSets FunctionSets {get; set;}
	

	}
}