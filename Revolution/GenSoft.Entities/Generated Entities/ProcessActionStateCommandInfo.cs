﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class ProcessActionStateCommandInfo: BaseEntity, IProcessActionStateCommandInfo
	{
		public virtual int StateComandInfoId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual ProcessAction ProcessAction {get; set;}
				public virtual StateCommandInfo StateCommandInfo {get; set;}
	

	}
}