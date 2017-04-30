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
	public static partial class FunctionReturnTypeExpressions
	{
		public static IQueryable<FunctionReturnType> GetFunctionReturnTypeById(this IQueryable<FunctionReturnType> query, int Id) => query.Where(x => x.Id == Id);


// Get Leaf Properties
    
		public static IQueryable<DataTypes> GetDataTypes(this IQueryable<FunctionReturnType> query) => query.DataTypes();
    
		public static IQueryable<Functions> GetFunctions(this IQueryable<FunctionReturnType> query) => query.Functions();
    
		public static IQueryable<FunctionReturnType> GetFunctionReturnType(this IQueryable<FunctionReturnType> query) => query;

			// Child Properties
			//Parent Properties
				//public static IQueryable<FunctionReturnType> FunctionReturnType(this IQueryable<DataTypes> datatypes) => datatypes.SelectMany(x => x.FunctionReturnType);
				public static IQueryable<DataTypes> DataTypes(this IQueryable<FunctionReturnType> query) => query.Select(x => x.DataTypes);
				//public static IQueryable<FunctionReturnType> FunctionReturnType(this IQueryable<Functions> functions) => functions.Select(x => x.FunctionReturnType);
				public static IQueryable<Functions> Functions(this IQueryable<FunctionReturnType> query) => query.Select(x => x.Functions);
	}
}
