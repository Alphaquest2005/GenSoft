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
	public partial class EntitiesAutoViewModel_AutoGen 
	{
		public object Applications  { get { return GetValue(); } set { SetValue(value); }}
		public object EntityViewProperties  { get { return GetValue(); } set { SetValue(value); }}
		public object EntityView  { get { return GetValue(); } set { SetValue(value); }}
		public object PresentationProperties  { get { return GetValue(); } set { SetValue(value); }}
		public object TestValues  { get { return GetValue(); } set { SetValue(value); }}
		public object EntityProperties  { get { return GetValue(); } set { SetValue(value); }}
		public object Entities  { get { return GetValue(); } set { SetValue(value); }}
		
		protected sealed override IEntitiesAutoView CreateNullEntity()
		{
			return new EntitiesAutoView(){Id = EntityStates.NullEntity};
		}
	
	}
}
