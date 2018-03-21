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
	public partial class ViewModelPropertyPresentationType: BaseEntity, IViewModelPropertyPresentationType
	{
		public virtual int PresentationThemeId { get; set; }
		public virtual int ValueOptionId { get; set; }
		public virtual int ViewModelTypeId { get; set; }
		public virtual int ViewPropertyPresentationPropertyTypeId { get; set; }
		public virtual int ViewTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual PresentationTheme PresentationTheme {get; set;}
				public virtual ViewPropertyValueOption ViewPropertyValueOption {get; set;}
				public virtual ViewModelType ViewModelType {get; set;}
				public virtual ViewPropertyPresentationPropertyType ViewPropertyPresentationPropertyType {get; set;}
				public virtual ViewType ViewType {get; set;}
	

	}
}
