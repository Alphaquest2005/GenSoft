﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class Command: BaseEntity, ICommand
	{
		public virtual int StateActionId { get; set; }
		public virtual int EntityId { get; set; }
		public virtual int CommandTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Entity Entity {get; set;}
	

	}
}
