﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class ActionReferenceTypes: BaseEntity, IActionReferenceTypes
	{
		public virtual int ActionId { get; set; }
		public virtual int ReferenceTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Action Action {get; set;}
				public virtual ReferenceTypes ReferenceTypes {get; set;}
	

	}
}