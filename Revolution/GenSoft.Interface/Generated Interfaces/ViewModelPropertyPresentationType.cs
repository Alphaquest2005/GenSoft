﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-Interfaces.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System.ComponentModel.Composition;


namespace GenSoft.Interfaces
{
	[InheritedExport]
	public partial interface IViewModelPropertyPresentationType:SystemInterfaces.IEntity  
	{
		int PresentationThemeId { get;}
		int ViewPropertyPresentationPropertyTypeId { get;}
		int ValueOptionId { get;}
		int ViewTypeId { get;}
		int ViewModelTypeId { get;}



	}
}
