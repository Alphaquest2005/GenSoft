﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System.Collections.Generic;
using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class Attributes: BaseEntity, IAttributes
	{
		public virtual int DataTypeId { get; set; }
		public virtual string Name { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<EntityTypeAttributes> EntityTypeAttributes {get; set;}
				public virtual ICollection<EntityAttribute> EntityAttribute {get; set;}
				public virtual EntityId EntityId {get; set;}
				public virtual EntityName EntityName {get; set;}
		
			// ---------Parent Relationships
				public virtual DataType DataType {get; set;}
	

	}
}
