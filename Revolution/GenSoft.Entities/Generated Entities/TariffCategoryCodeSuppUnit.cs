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
	public partial class TariffCategoryCodeSuppUnit: BaseEntity, ITariffCategoryCodeSuppUnit
	{
		public virtual int TariffSupUnitId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual ICollection<TariffCategory> TariffCategory {get; set;}
				public virtual TariffSupUnitLkps TariffSupUnitLkps {get; set;}
	

	}
}