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
	public partial class Suppliers: BaseEntity, ISuppliers
	{
		public virtual string City { get; set; }
		public virtual string Country { get; set; }
		public virtual string Street { get; set; }
		public virtual string SupplierCode { get; set; }
		public virtual string SupplierName { get; set; }
		public virtual string ZipCode { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
	

	}
}
