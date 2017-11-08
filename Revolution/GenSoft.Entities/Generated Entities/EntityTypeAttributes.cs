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
				public virtual ICollection<ActionParameterEntityTypeAttributes> ActionParameterEntityTypeAttributes {get; set;}
				public virtual ActionProperties ActionProperties {get; set;}
				public virtual ICollection<BaseEntityTypeAttribute> BaseEntityTypeAttributes {get; set;}
				public virtual BaseEntityTypeAttribute BaseEntityTypeAttribute {get; set;}
				public virtual CalculatedProperties CalculatedProperties {get; set;}
				public virtual ICollection<CalculatedPropertyParameterEntityTypes> CalculatedPropertyParameterEntityTypes {get; set;}
				public virtual ICollection<EntityRelationship> EntityRelationship {get; set;}
				public virtual EntityTypeAttributeCache EntityTypeAttributeCache {get; set;}
				public virtual ICollection<EntityTypePresentationProperty> EntityTypePresentationProperty {get; set;}
				public virtual ICollection<ParentEntity> ParentEntity {get; set;}
		
			// ---------Parent Relationships
				public virtual Attributes Attributes {get; set;}
				public virtual EntityType EntityType {get; set;}
	

	}
}
