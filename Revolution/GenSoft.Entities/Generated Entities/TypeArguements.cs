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
	public partial class TypeArguements: BaseEntity, ITypeArguements
	{
		public virtual int ChildTypeId { get; set; }
		public virtual int ParentTypeId { get; set; }
		public virtual int TypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Type ChildType {get; set;}
				public virtual Type ParentType {get; set;}
				public virtual Type Type {get; set;}
	

	}
}
