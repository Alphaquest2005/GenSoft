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
	public partial class xcuda_Container: BaseEntity, Ixcuda_Container
	{
		public virtual int ASYCUDA_Id { get; set; }
		public virtual string Container_identity { get; set; }
		public virtual string Container_type { get; set; }
		public virtual string Empty_full_indicator { get; set; }
		public virtual string Goods_description { get; set; }
		public virtual double Gross_weight { get; set; }
		public virtual string Item_Number { get; set; }
		public virtual string Packages_number { get; set; }
		public virtual string Packages_type { get; set; }
		public virtual double Packages_weight { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_ASYCUDA xcuda_ASYCUDA {get; set;}
	

	}
}
