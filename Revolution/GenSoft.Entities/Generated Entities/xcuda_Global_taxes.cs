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
	public partial class xcuda_Global_taxes: BaseEntity, Ixcuda_Global_taxes
	{
		public virtual int ASYCUDA_Id { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_ASYCUDA xcuda_ASYCUDA {get; set;}
	

	}
}