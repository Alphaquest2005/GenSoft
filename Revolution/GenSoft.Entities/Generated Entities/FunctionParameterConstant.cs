﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class FunctionParameterConstant: BaseEntity, IFunctionParameterConstant
	{
		public virtual int CalculatedPropertyId { get; set; }
		public virtual int FunctionParameterId { get; set; }
		public virtual string Value { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual CalculatedProperties CalculatedProperties {get; set;}
				public virtual FunctionParameter FunctionParameter {get; set;}
	

	}
}
