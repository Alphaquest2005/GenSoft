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
	public partial class xcuda_Delivery_terms: BaseEntity, Ixcuda_Delivery_terms
	{
		public virtual string Code { get; set; }
		public virtual string Place { get; set; }
		public virtual string Situation { get; set; }
		public virtual int Transport_Id { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_Transport xcuda_Transport {get; set;}
	

	}
}