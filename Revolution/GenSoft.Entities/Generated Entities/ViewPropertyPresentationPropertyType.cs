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
	public partial class ViewPropertyPresentationPropertyType: BaseEntity, IViewPropertyPresentationPropertyType
	{
		public virtual int PresentationPropertyTypeId { get; set; }
		public virtual int ViewPropertyId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<EntityTypePresentationProperty> EntityTypePresentationProperty {get; set;}
				public virtual ICollection<ViewModelPropertyPresentationType> ViewModelPropertyPresentationType {get; set;}
				public virtual ICollection<ViewPropertyTheme> ViewPropertyTheme {get; set;}
		
			// ---------Parent Relationships
				public virtual PresentationPropertyType PresentationPropertyType {get; set;}
				public virtual ViewProperty ViewProperty {get; set;}
	

	}
}
