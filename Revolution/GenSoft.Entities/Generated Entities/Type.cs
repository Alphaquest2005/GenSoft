﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using Common.DataEntites;
using GenSoft.Entities;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class Type: BaseEntity, IType
	{
		public virtual string Name { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual DataType DataType {get; set;}
				public virtual ICollection<EntityType> EntityType {get; set;}
				public virtual EventType EventType {get; set;}
				public virtual SourceType SourceType {get; set;}
				public virtual ICollection<TypeArguement> ChildTypes {get; set;}
				public virtual ICollection<TypeArguement> ParentTypes {get; set;}
				public virtual ICollection<TypeArguement> Types {get; set;}
		
			// ---------Parent Relationships
	

	}
}
