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
	public partial class EntityTypeAttribute: BaseEntity, IEntityTypeAttribute
	{
		public virtual int AttributeId { get; set; }
		public virtual int EntityTypeId { get; set; }
		public virtual int Priority { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<ActionParameterEntityTypeAttribute> ActionParameterEntityTypeAttributes {get; set;}
				public virtual ActionProperty ActionProperty {get; set;}
				public virtual ICollection<BaseEntityTypeAttribute> Base_EntityTypeAttributes {get; set;}
				public virtual BaseEntityTypeAttribute BaseEntityTypeAttribute {get; set;}
				public virtual CalculatedProperty CalculatedProperty {get; set;}
				public virtual ICollection<CalculatedPropertyParameterEntityType> CalculatedPropertyParameterEntityTypes {get; set;}
				public virtual EntityName EntityName {get; set;}
				public virtual ICollection<EntityRelationship> EntityRelationships {get; set;}
				public virtual EntityTypeAttributeCach EntityTypeAttributeCach {get; set;}
				public virtual ICollection<EntityTypePresentationProperty> EntityTypePresentationProperties {get; set;}
				public virtual ICollection<ParentEntity> ParentEntities {get; set;}
		
			// ---------Parent Relationships
				public virtual Attribute Attribute {get; set;}
				public virtual EntityType EntityType {get; set;}
	

	}
}