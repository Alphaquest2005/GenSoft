﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System.Collections.Generic;
using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class EntityTypePresentationProperty: BaseEntity, IEntityTypePresentationProperty
	{
		public virtual int EntityTypeAttributeId { get; set; }
		public virtual int PresentationThemeId { get; set; }
		public virtual int ViewPropertyPresentationPropertyTypeId { get; set; }
		public virtual int ViewTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual PropertyValue PropertyValue {get; set;}
				public virtual ICollection<EntityView> EntityView {get; set;}
				public virtual PropertyValueOption PropertyValueOption {get; set;}
		
			// ---------Parent Relationships
				public virtual EntityTypeAttributes EntityTypeAttributes {get; set;}
				public virtual PresentationTheme PresentationTheme {get; set;}
				public virtual ViewPropertyPresentationPropertyType ViewPropertyPresentationPropertyType {get; set;}
				public virtual ViewType ViewType {get; set;}
	

	}
}
