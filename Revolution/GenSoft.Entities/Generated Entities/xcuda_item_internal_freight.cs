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
	public partial class xcuda_item_internal_freight: BaseEntity, Ixcuda_item_internal_freight
	{
		public virtual double Amount_foreign_currency { get; set; }
		public virtual double Amount_national_currency { get; set; }
		public virtual string Currency_name { get; set; }
		public virtual double Currency_rate { get; set; }
		public virtual int Valuation_item_Id { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_Valuation_item xcuda_Valuation_item {get; set;}
	

	}
}
