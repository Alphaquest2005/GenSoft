﻿// <autogenerated>
//   This file was generated by T4 code generator Amoeba-Master.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using System.Linq;
using Common.DataEntites;
using EF.Entities;
using Interfaces;

namespace Entity.Expressions
{
	public static partial class PresentationPropertiesExpressions
	{
		public static IQueryable<PresentationProperties> GetPresentationPropertiesById(this IQueryable<PresentationProperties> query, int Id) => query.Where(x => x.Id == Id);


// Get Leaf Properties
    
		public static IQueryable<PresentationProperties> GetPresentationProperties(this IQueryable<PresentationProperties> query) => query;

			// Child Properties
			//Parent Properties
				//public static IQueryable<PresentationProperties> PresentationProperties(this IQueryable<EntityProperties> entityproperties) => entityproperties.SelectMany(x => x.PresentationProperties);
				public static IQueryable<EntityProperties> EntityProperties(this IQueryable<PresentationProperties> query) => query.Select(x => x.EntityProperties);
	}
}
