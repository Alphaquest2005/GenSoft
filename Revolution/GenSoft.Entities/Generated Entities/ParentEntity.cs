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
	public partial class ParentEntity: BaseEntity, IParentEntity
	{
		public virtual int ParentEntityId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual EntityRelationship EntityRelationship {get; set;}
				public virtual EntityTypeAttribute EntityTypeAttribute {get; set;}
	

	}
}
