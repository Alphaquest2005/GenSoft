﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-Interfaces.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System.ComponentModel.Composition;


namespace GenSoft.Interfaces
{
	[InheritedExport]
	public partial interface IProcessAction:SystemInterfaces.IEntity  
	{
		int ActionId { get;}
		string Name { get;}



	}
}
