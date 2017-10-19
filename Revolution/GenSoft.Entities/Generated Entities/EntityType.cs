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
	public partial class EntityType: BaseEntity, IEntityType
	{
		public virtual int ApplicationId { get; set; }
		public virtual string EntitySetName { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual DBType DBType {get; set;}
				public virtual ICollection<Entity> Entity {get; set;}
				public virtual ICollection<EntityTypeAttributes> EntityTypeAttributes {get; set;}
				public virtual ICollection<EntityTypeViewModelCommand> EntityTypeViewModelCommand {get; set;}
				public virtual ICollection<MainEntity> MainEntity {get; set;}
		
			// ---------Parent Relationships
				public virtual Application Application {get; set;}
				public virtual Type Type {get; set;}
	

	}
}
