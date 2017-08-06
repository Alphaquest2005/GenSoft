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
		public virtual int DataTypeId { get; set; }
		public virtual string Name { get; set; }
		public virtual int EntityTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual EntityId EntityId {get; set;}
				public virtual EntityName EntityName {get; set;}
				public virtual ICollection<EntityRelationships> ParentEntitys {get; set;}
				public virtual ICollection<EntityRelationships> ChildEntitys {get; set;}
		
			// ---------Parent Relationships
				public virtual DataType DataType {get; set;}
				public virtual EntityType EntityType {get; set;}
	

	}
}
