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
	public partial class xcuda_Traders_Financial: BaseEntity, Ixcuda_Traders_Financial
	{
		public virtual int Traders_Id { get; set; }
		public virtual string Financial_code { get; set; }
		public virtual string Financial_name { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_Traders xcuda_Traders {get; set;}
	

	}
}
