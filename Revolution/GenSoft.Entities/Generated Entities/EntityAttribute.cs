﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using Common.DataEntites;
using System.Linq;
using GenSoft.Entities;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class EntityAttribute: BaseEntity<EntityAttribute>, IEntityAttribute
	{
		public virtual int EntityId { get; set; }
		public virtual int DataTypeId { get; set; }
		public virtual string Name { get; set; }
		public virtual string Value { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Entity Entity {get; set;}
				//IEntity IEntityAttribute.Entity => Entity;
	

	}
}
