﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.DataEntites;
using EF.Entities;
using Interfaces;

namespace Entity.Expressions
{
	public static partial class StateMachineStatesExpressions
	{

		public static Expression<Func<StateMachineStates, StateMachineStatesAutoView>> StateMachineStatesAutoViewExpression { get; } =
		
			x => new StateMachineStatesAutoView 
			{
				Id = x.Id,
 				StateMachines = x.StateMachines.Name,   
 				States = x.States.Name,   
			};
	}
}
