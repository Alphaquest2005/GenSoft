﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using Core.Common.UI;
using EF.Entities;
using DataInterfaces;
using Interfaces;
using Utilities;

namespace ViewModels
{
	public partial class FunctionsAutoViewModel_AutoGen 
	{
		public object EntityViewProperties  { get { return GetValue(); } set { SetValue(value); }}
		public object EntityViewPropertyFunctionParameter  { get { return GetValue(); } set { SetValue(value); }}
		public object FunctionParametersEntityViewPropertyFunctionParameter  { get { return GetValue(); } set { SetValue(value); }}
		public object Parameters  { get { return GetValue(); } set { SetValue(value); }}
		public object DataTypes  { get { return GetValue(); } set { SetValue(value); }}
		public object Functions  { get { return GetValue(); } set { SetValue(value); }}
		
		protected sealed override IFunctionsAutoView CreateNullEntity()
		{
			return new FunctionsAutoView(){Id = EntityStates.NullEntity};
		}
	
	}
}
