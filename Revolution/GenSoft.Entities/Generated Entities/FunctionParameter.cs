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
	public partial class FunctionParameter: BaseEntity, IFunctionParameter
	{
		public virtual int DataTypeId { get; set; }
		public virtual int FunctionId { get; set; }
		public virtual string Name { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<CalculatedPropertyParameter> CalculatedPropertyParameters {get; set;}
				public virtual ICollection<FunctionParameterConstant> FunctionParameterConstants {get; set;}
		
			// ---------Parent Relationships
				public virtual DataType DataType {get; set;}
				public virtual Function Function {get; set;}
	

	}
}
