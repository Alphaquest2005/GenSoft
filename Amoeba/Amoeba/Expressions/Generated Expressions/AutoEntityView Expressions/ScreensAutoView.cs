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
	public static partial class ScreensExpressions
	{

		public static Expression<Func<Screens, ScreensAutoView>> ScreensAutoViewExpression { get; } =
		
			x => new ScreensAutoView 
			{
				Id = x.Id,
 				Layout = x.ScreenLayouts.Select(x0 => x0.Layout).Select(z => z.Name).FirstOrDefault(),   
 				Parts = x.ScreenParts.Select(x0 => x0.Parts).Select(z => z.Name).FirstOrDefault(),   
 				Screens = x.Name,   
			};
	}
}
