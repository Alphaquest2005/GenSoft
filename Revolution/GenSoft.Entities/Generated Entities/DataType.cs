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
	public partial class DataType: BaseEntity, IDataType
	{

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<Attributes> Attributes {get; set;}
		
			// ---------Parent Relationships
				public virtual Type Type {get; set;}
	

	}
}
