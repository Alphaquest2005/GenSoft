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
	public partial class RelationshipType: BaseEntity, IRelationshipType
	{
		public virtual int ChildOrdinalityId { get; set; }
		public virtual string Name { get; set; }
		public virtual int ParentOrdinalityId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<EntityRelationship> EntityRelationship {get; set;}
		
			// ---------Parent Relationships
				public virtual Ordinality ChildOrdinalitys {get; set;}
				public virtual Ordinality ParentOrdinalitys {get; set;}
	

	}
}
