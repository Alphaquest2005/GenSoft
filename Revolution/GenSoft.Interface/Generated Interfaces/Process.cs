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
	public partial interface IProcess:SystemInterfaces.IEntity  
	{
		int UserId { get;}
		string Description { get;}
		string Name { get;}
		int ParentProcessId { get;}
		string Symbol { get;}



	}
}
