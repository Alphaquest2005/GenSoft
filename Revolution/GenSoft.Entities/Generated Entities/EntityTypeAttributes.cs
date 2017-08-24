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
	public partial class EntityTypeAttributes: BaseEntity, IEntityTypeAttributes
	{
		public virtual int AttributeId { get; set; }
		public virtual int EntityTypeId { get; set; }
		public virtual int Priority { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<EntityRelationships> ParentEntitys {get; set;}
				public virtual ICollection<EntityRelationships> ChildEntitys {get; set;}
		
			// ---------Parent Relationships
				public virtual Attributes Attributes {get; set;}
				public virtual EntityType EntityType {get; set;}
	

	}
}
