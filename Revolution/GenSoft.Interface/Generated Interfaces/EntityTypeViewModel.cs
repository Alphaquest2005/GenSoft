﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-Interfaces.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using GenSoft.Interfaces;


namespace GenSoft.Interfaces
{
	[InheritedExport]
	public partial interface IEntityTypeViewModel:SystemInterfaces.IEntity  
	{
		string Description { get;}
		int Priority { get;}
		int ProcessDomainEntityTypeId { get;}
		string PropertyName { get;}
		string Symbol { get;}
		int ViewModelId { get;}



	}
}
